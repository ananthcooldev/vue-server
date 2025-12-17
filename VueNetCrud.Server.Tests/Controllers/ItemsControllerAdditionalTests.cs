using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Interfaces;
using VueNetCrud.Server.Controllers;
using Xunit;

namespace VueNetCrud.Server.Tests.Controllers;

public class ItemsControllerAdditionalTests
{
    private readonly Mock<IItemService> _mockItemService;
    private readonly Mock<ILogger<ItemsController>> _mockLogger;
    private readonly ItemsController _controller;

    public ItemsControllerAdditionalTests()
    {
        _mockItemService = new Mock<IItemService>();
        _mockLogger = new Mock<ILogger<ItemsController>>();
        _controller = new ItemsController(_mockItemService.Object, _mockLogger.Object);
        
        // Set up authorization context
        var claims = new[] { new Claim(ClaimTypes.Name, "testuser") };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
        {
            HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
            {
                User = principal
            }
        };
    }

    [Fact]
    public async Task GetAll_ShouldLogInformation()
    {
        // Arrange
        _mockItemService.Setup(s => s.GetAllAsync()).ReturnsAsync(Enumerable.Empty<ItemResponseDto>());

        // Act
        await _controller.GetAll();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public async Task GetById_ShouldLogInformation()
    {
        // Arrange
        var item = new ItemResponseDto(1, "Test Item", "Description");
        _mockItemService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(item);

        // Act
        await _controller.GetById(1);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public async Task Create_ShouldLogInformation()
    {
        // Arrange
        var dto = new ItemCreateDto("New Item", "Description");
        var created = new ItemResponseDto(1, "New Item", "Description");
        _mockItemService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

        // Act
        await _controller.Create(dto);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public async Task Update_ShouldLogInformation()
    {
        // Arrange
        var dto = new ItemUpdateDto("Updated", "New Description");
        var updated = new ItemResponseDto(1, "Updated", "New Description");
        _mockItemService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(updated);

        // Act
        await _controller.Update(1, dto);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldLogInformation()
    {
        // Arrange
        _mockItemService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        await _controller.Delete(1);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public void TestError_ShouldLogInformation()
    {
        // Act & Assert
        Assert.Throws<Exception>(() => _controller.TestError());
        
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }
}
