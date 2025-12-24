using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Interfaces;
using VueNetCrud.Server.Domain.Interfaces.Services;

namespace VueNetCrud.Server.Application.Services;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(ITokenService tokenService, ILogger<AuthService> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    public Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        // Dummy validation — replace with DB check later
        if (request.Username == "admin" && request.Password == "123")
        {
            var token = _tokenService.GenerateToken(request.Username);
            _logger.LogInformation("User {Username} logged in successfully", request.Username);
            return Task.FromResult<LoginResponseDto?>(new LoginResponseDto(token));
        }

        _logger.LogWarning("Failed login attempt for username {Username}", request.Username);
        return Task.FromResult<LoginResponseDto?>(null);
    }
}

