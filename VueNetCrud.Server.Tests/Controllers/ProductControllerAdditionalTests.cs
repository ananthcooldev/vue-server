using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Interfaces;
using VueNetCrud.Server.Controllers;
using Xunit;

namespace VueNetCrud.Server.Tests.Controllers;

public class ProductControllerAdditionalTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly Mock<ILogger<ProductController>> _mockLogger;
    private readonly ProductController _controller;

    public ProductControllerAdditionalTests()
    {
        _mockProductService = new Mock<IProductService>();
        _mockLogger = new Mock<ILogger<ProductController>>();
        _controller = new ProductController(_mockProductService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAll_WithEmptyList_ShouldReturnOk()
    {
        // Arrange
        _mockProductService.Setup(s => s.GetAllAsync()).ReturnsAsync(Enumerable.Empty<ProductResponseDto>());

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var products = okResult!.Value as IEnumerable<ProductResponseDto>;
        products.Should().BeEmpty();
    }

    [Fact]
    public async Task Add_ShouldLogInformation()
    {
        // Arrange
        var dto = new ProductCreateDto("New Product", 100, "Electronics");
        var created = new ProductResponseDto(1, "New Product", 100, "Electronics");
        _mockProductService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

        // Act
        await _controller.Add(dto);

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
        var dto = new ProductUpdateDto(1, "Updated", 100, "Electronics");
        _mockProductService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(true);

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
    public async Task Delete_ShouldLogWarning()
    {
        // Arrange
        _mockProductService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        await _controller.Delete(1);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }
}
