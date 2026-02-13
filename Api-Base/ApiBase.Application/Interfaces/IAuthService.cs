using ApiBase.Application.DTOs;

namespace ApiBase.Application.Interfaces;

public interface IAuthService
{
    Task Register(RegisterDto dto);
    Task<string> Login(string email, string password);
}