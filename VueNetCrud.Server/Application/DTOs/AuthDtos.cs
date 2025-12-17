namespace VueNetCrud.Server.Application.DTOs;

public record LoginRequestDto(string Username, string Password);
public record LoginResponseDto(string Token);

