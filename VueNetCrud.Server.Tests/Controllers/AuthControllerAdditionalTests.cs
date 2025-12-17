using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Interfaces;
using VueNetCrud.Server.Controllers;
using Xunit;

namespace VueNetCrud.Server.Tests.Controllers;

public class AuthControllerAdditionalTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly Mock<ILogger<AuthController>> _mockLogger;
    private readonly AuthController _controller;

    public AuthControllerAdditionalTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _mockLogger = new Mock<ILogger<AuthController>>();
        _controller = new AuthController(_mockAuthService.Object, _mockLogger.Object);
    }

    [Fact]
    public void Options_WithLoginRoute_ShouldReturnOk()
    {
        // Act
        var result = _controller.Options();

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task Login_WithEmptyUsername_ShouldReturnUnauthorized()
    {
        // Arrange
        var request = new LoginRequestDto("", "123");
        _mockAuthService.Setup(s => s.LoginAsync(request)).ReturnsAsync((LoginResponseDto?)null);

        // Act
        var result = await _controller.Login(request);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult!.Value.Should().Be("Invalid credentials");
    }

    [Fact]
    public async Task Login_WithEmptyPassword_ShouldReturnUnauthorized()
    {
        // Arrange
        var request = new LoginRequestDto("admin", "");
        _mockAuthService.Setup(s => s.LoginAsync(request)).ReturnsAsync((LoginResponseDto?)null);

        // Act
        var result = await _controller.Login(request);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult!.Value.Should().Be("Invalid credentials");
    }

    [Fact]
    public async Task Login_WithWhitespaceCredentials_ShouldReturnUnauthorized()
    {
        // Arrange
        var request = new LoginRequestDto("   ", "   ");
        _mockAuthService.Setup(s => s.LoginAsync(request)).ReturnsAsync((LoginResponseDto?)null);

        // Act
        var result = await _controller.Login(request);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task Login_ShouldGenerateValidJwtToken()
    {
        // Arrange
        var request = new LoginRequestDto("admin", "123");
        var response = new LoginResponseDto("test-token-123");
        _mockAuthService.Setup(s => s.LoginAsync(request)).ReturnsAsync(response);

        // Act
        var result = await _controller.Login(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().NotBeNull();
        
        var loginResponse = okResult.Value as LoginResponseDto;
        loginResponse.Should().NotBeNull();
        loginResponse!.Token.Should().Be("test-token-123");
    }
}
