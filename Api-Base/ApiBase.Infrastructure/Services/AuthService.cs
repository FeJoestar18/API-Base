using ApiBase.Application.DTOs;
using ApiBase.Application.Interfaces;
using ApiBase.Application.Common;
using ApiBase.Domain.Entities;
using ApiBase.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiBase.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly UserRepository _repo;
    private readonly IViaCepService _viaCep;

    public AuthService(IConfiguration config, UserRepository repo, IViaCepService viaCep)
    {
        _config = config;
        _repo = repo;
        _viaCep = viaCep;
    }

    public async Task Register(RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Cpf) || dto.Cpf.Length != 11 || !dto.Cpf.All(char.IsDigit))
            throw new Exception(Messages.InvalidCpf);

        if (dto.Name is null || dto.Name.Length > 45)
            throw new Exception(Messages.InvalidName);

        var existingCpf = await _repo.GetByCpf(dto.Cpf);
        if (existingCpf is not null)
            throw new Exception(Messages.CpfAlreadyRegistered);

        var existingEmail = await _repo.GetByEmail(dto.Email);
        if (existingEmail is not null)
            throw new Exception(Messages.EmailAlreadyRegistered);

        var cep = dto.Address.Cep;
        if (!await _viaCep.CepExistsAsync(cep))
            throw new Exception(Messages.InvalidCep);

        var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = hash,
            Cpf = dto.Cpf,
            Name = dto.Name,
        };

        user.Addresses.Add(new UserAddress
        {
            Id = Guid.NewGuid(),
            Cep = dto.Address.Cep,
            Street = dto.Address.Street,
            Number = dto.Address.Number,
            Complement = dto.Address.Complement,
            Neighborhood = dto.Address.Neighborhood,
            City = dto.Address.City,
            State = dto.Address.State,
            User = user
        });

        await _repo.Add(user);
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await _repo.GetByEmail(email)
                   ?? throw new Exception(Messages.UserNotFound);

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new Exception(Messages.InvalidPassword);

        return GenerateToken(user);
    }

    private string GenerateToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);

        var handler = new JwtSecurityTokenHandler();

        var token = handler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        });

        return handler.WriteToken(token);
    }
}