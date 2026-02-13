using ApiBase.Application.DTOs;
using ApiBase.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiBase.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        await _service.Register(dto.Email, dto.Password);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _service.Login(dto.Email, dto.Password);
        return Ok(new { token });
    }
}