namespace VueNetCrud.Server.Application.DTOs;

public record ProductCreateDto(string Name, decimal Price, string Category);
public record ProductUpdateDto(int Id, string Name, decimal Price, string Category);
public record ProductResponseDto(int Id, string Name, decimal Price, string Category);

