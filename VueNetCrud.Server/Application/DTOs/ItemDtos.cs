namespace VueNetCrud.Server.Application.DTOs;

public record ItemCreateDto(string Name, string? Description);
public record ItemUpdateDto(string? Name, string? Description);
public record ItemResponseDto(int Id, string Name, string? Description);

