using ApiBase.Application.Interfaces;
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

    public AuthService(IConfiguration config, UserRepository repo)
    {
        _config = config;
        _repo = repo;
    }

    public async Task Register(string email, string password)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(password);

        await _repo.Add(new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = hash
        });
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await _repo.GetByEmail(email)
                   ?? throw new Exception("Usuário não encontrado");

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new Exception("Senha inválida");

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