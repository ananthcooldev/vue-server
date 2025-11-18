namespace VueNetCrud.Server.Models;

public record Item(int id, string name, string? description);
public record ItemCreate(string name, string? description);
public record ItemUpdate(string? name, string? description);


