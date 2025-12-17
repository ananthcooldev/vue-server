using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using VueNetCrud.Server.Infrastructure.Services;
using Xunit;

namespace VueNetCrud.Server.Tests.Infrastructure.Services;

public class TokenServiceTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IConfigurationSection> _mockJwtSection;
    private readonly TokenService _service;

    public TokenServiceTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockJwtSection = new Mock<IConfigurationSection>();

        _mockJwtSection.Setup(x => x["Key"]).Returns("ThisIsASecretKeyForJwtTokenGeneration123456");
        _mockJwtSection.Setup(x => x["Issuer"]).Returns("TestIssuer");
        _mockJwtSection.Setup(x => x["Audience"]).Returns("TestAudience");

        _mockConfiguration.Setup(x => x.GetSection("Jwt")).Returns(_mockJwtSection.Object);

        _service = new TokenService(_mockConfiguration.Object);
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidJwtToken()
    {
        // Act
        var token = _service.GenerateToken("testuser");

        // Assert
        token.Should().NotBeNullOrEmpty();
        
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        jsonToken.Should().NotBeNull();
    }

    [Fact]
    public void GenerateToken_ShouldContainUsernameClaim()
    {
        // Act
        var token = _service.GenerateToken("testuser");

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        jsonToken.Claims.Should().Contain(c => c.Type == System.Security.Claims.ClaimTypes.Name && c.Value == "testuser");
    }

    [Fact]
    public void GenerateToken_ShouldHaveCorrectIssuer()
    {
        // Act
        var token = _service.GenerateToken("testuser");

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        jsonToken.Issuer.Should().Be("TestIssuer");
    }

    [Fact]
    public void GenerateToken_ShouldHaveCorrectAudience()
    {
        // Act
        var token = _service.GenerateToken("testuser");

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        jsonToken.Audiences.Should().Contain("TestAudience");
    }

    [Fact]
    public void GenerateToken_ShouldHaveExpiration()
    {
        // Act
        var token = _service.GenerateToken("testuser");

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        jsonToken.ValidTo.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public void GenerateToken_WithDifferentUsernames_ShouldGenerateDifferentTokens()
    {
        // Act
        var token1 = _service.GenerateToken("user1");
        var token2 = _service.GenerateToken("user2");

        // Assert
        token1.Should().NotBe(token2);
    }

    [Fact]
    public void GenerateToken_WithNullKey_ShouldThrowException()
    {
        // Arrange
        _mockJwtSection.Setup(x => x["Key"]).Returns((string?)null);
        var service = new TokenService(_mockConfiguration.Object);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => service.GenerateToken("testuser"));
    }
}

