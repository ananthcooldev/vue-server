using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Services;
using VueNetCrud.Server.Domain.Interfaces.Services;
using Xunit;

namespace VueNetCrud.Server.Tests.Application.Services;

public class AuthServiceTests
{
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<ILogger<AuthService>> _mockLogger;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _mockTokenService = new Mock<ITokenService>();
        _mockLogger = new Mock<ILogger<AuthService>>();
        _service = new AuthService(_mockTokenService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var request = new LoginRequestDto("admin", "123");
        var expectedToken = "test-token-123";
        _mockTokenService.Setup(t => t.GenerateToken("admin")).Returns(expectedToken);

        // Act
        var result = await _service.LoginAsync(request);

        // Assert
        result.Should().NotBeNull();
        result!.Token.Should().Be(expectedToken);
        _mockTokenService.Verify(t => t.GenerateToken("admin"), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidUsername_ShouldReturnNull()
    {
        // Arrange
        var request = new LoginRequestDto("invalid", "123");

        // Act
        var result = await _service.LoginAsync(request);

        // Assert
        result.Should().BeNull();
        _mockTokenService.Verify(t => t.GenerateToken(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ShouldReturnNull()
    {
        // Arrange
        var request = new LoginRequestDto("admin", "wrongpassword");

        // Act
        var result = await _service.LoginAsync(request);

        // Assert
        result.Should().BeNull();
        _mockTokenService.Verify(t => t.GenerateToken(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WithEmptyUsername_ShouldReturnNull()
    {
        // Arrange
        var request = new LoginRequestDto("", "123");

        // Act
        var result = await _service.LoginAsync(request);

        // Assert
        result.Should().BeNull();
        _mockTokenService.Verify(t => t.GenerateToken(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WithEmptyPassword_ShouldReturnNull()
    {
        // Arrange
        var request = new LoginRequestDto("admin", "");

        // Act
        var result = await _service.LoginAsync(request);

        // Assert
        result.Should().BeNull();
        _mockTokenService.Verify(t => t.GenerateToken(It.IsAny<string>()), Times.Never);
    }
}

