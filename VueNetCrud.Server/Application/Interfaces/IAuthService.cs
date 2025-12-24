using VueNetCrud.Server.Application.DTOs;

namespace VueNetCrud.Server.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
}

