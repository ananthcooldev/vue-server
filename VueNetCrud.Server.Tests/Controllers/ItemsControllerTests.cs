using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Interfaces;
using VueNetCrud.Server.Controllers;
using Xunit;

namespace VueNetCrud.Server.Tests.Controllers;

public class ItemsControllerTests
{
    private readonly Mock<IItemService> _mockItemService;
    private readonly Mock<ILogger<ItemsController>> _mockLogger;
    private readonly ItemsController _controller;

    public ItemsControllerTests()
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
    public async Task GetAll_ShouldReturnOkWithItems()
    {
        // Arrange
        var items = new List<ItemResponseDto>
        {
            new ItemResponseDto(1, "Item 1", "Description 1"),
            new ItemResponseDto(2, "Item 2", "Description 2")
        };
        _mockItemService.Setup(s => s.GetAllAsync()).ReturnsAsync(items);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        _mockItemService.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetById_WithValidId_ShouldReturnOk()
    {
        // Arrange
        var item = new ItemResponseDto(1, "Item 1", "Description 1");
        _mockItemService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(item);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        _mockItemService.Verify(s => s.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        _mockItemService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((ItemResponseDto?)null);

        // Act
        var result = await _controller.GetById(999);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
        _mockItemService.Verify(s => s.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task Create_WithValidDto_ShouldReturnCreated()
    {
        // Arrange
        var dto = new ItemCreateDto("New Item", "New Description");
        var created = new ItemResponseDto(1, "New Item", "New Description");
        _mockItemService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

        // Act
        var result = await _controller.Create(dto);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        _mockItemService.Verify(s => s.CreateAsync(dto), Times.Once);
    }

    [Fact]
    public async Task Create_WithInvalidDto_ShouldReturnBadRequest()
    {
        // Arrange
        var dto = new ItemCreateDto("", "Description");
        _mockItemService.Setup(s => s.CreateAsync(dto)).ThrowsAsync(new ArgumentException("Name is required"));

        // Act
        var result = await _controller.Create(dto);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
        _mockItemService.Verify(s => s.CreateAsync(dto), Times.Once);
    }

    [Fact]
    public async Task Update_WithValidId_ShouldReturnOk()
    {
        // Arrange
        var dto = new ItemUpdateDto("Updated Name", "Updated Description");
        var updated = new ItemResponseDto(1, "Updated Name", "Updated Description");
        _mockItemService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(updated);

        // Act
        var result = await _controller.Update(1, dto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        _mockItemService.Verify(s => s.UpdateAsync(1, dto), Times.Once);
    }

    [Fact]
    public async Task Update_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var dto = new ItemUpdateDto("Updated Name", "Updated Description");
        _mockItemService.Setup(s => s.UpdateAsync(999, dto)).ReturnsAsync((ItemResponseDto?)null);

        // Act
        var result = await _controller.Update(999, dto);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
        _mockItemService.Verify(s => s.UpdateAsync(999, dto), Times.Once);
    }

    [Fact]
    public async Task Delete_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        _mockItemService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _mockItemService.Verify(s => s.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task Delete_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        _mockItemService.Setup(s => s.DeleteAsync(999)).ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _mockItemService.Verify(s => s.DeleteAsync(999), Times.Once);
    }

    [Fact]
    public void TestError_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<Exception>(() => _controller.TestError());
    }
}
