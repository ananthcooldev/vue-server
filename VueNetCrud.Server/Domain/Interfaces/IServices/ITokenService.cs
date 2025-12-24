namespace VueNetCrud.Server.Domain.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(string username);
}

