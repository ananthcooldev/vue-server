using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Interfaces;
using VueNetCrud.Server.Controllers;
using Xunit;

namespace VueNetCrud.Server.Tests.Controllers;

public class ProductControllerTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly Mock<ILogger<ProductController>> _mockLogger;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _mockProductService = new Mock<IProductService>();
        _mockLogger = new Mock<ILogger<ProductController>>();
        _controller = new ProductController(_mockProductService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkWithProducts()
    {
        // Arrange
        var products = new List<ProductResponseDto>
        {
            new ProductResponseDto(1, "Product 1", 100, "Electronics"),
            new ProductResponseDto(2, "Product 2", 200, "Books")
        };
        _mockProductService.Setup(s => s.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _mockProductService.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetById_WithValidId_ShouldReturnOk()
    {
        // Arrange
        var product = new ProductResponseDto(1, "Product 1", 100, "Electronics");
        _mockProductService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _mockProductService.Verify(s => s.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        _mockProductService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((ProductResponseDto?)null);

        // Act
        var result = await _controller.GetById(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _mockProductService.Verify(s => s.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task Add_WithValidDto_ShouldReturnCreated()
    {
        // Arrange
        var dto = new ProductCreateDto("New Product", 150, "Electronics");
        var created = new ProductResponseDto(1, "New Product", 150, "Electronics");
        _mockProductService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

        // Act
        var result = await _controller.Add(dto);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        _mockProductService.Verify(s => s.CreateAsync(dto), Times.Once);
    }

    [Fact]
    public async Task Update_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        var dto = new ProductUpdateDto(1, "Updated Product", 200, "Books");
        _mockProductService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(true);

        // Act
        var result = await _controller.Update(1, dto);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _mockProductService.Verify(s => s.UpdateAsync(1, dto), Times.Once);
    }

    [Fact]
    public async Task Update_WithMismatchedId_ShouldReturnBadRequest()
    {
        // Arrange
        var dto = new ProductUpdateDto(2, "Updated Product", 200, "Books");

        // Act
        var result = await _controller.Update(1, dto);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
        _mockProductService.Verify(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<ProductUpdateDto>()), Times.Never);
    }

    [Fact]
    public async Task Update_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var dto = new ProductUpdateDto(999, "Updated Product", 200, "Books");
        _mockProductService.Setup(s => s.UpdateAsync(999, dto)).ReturnsAsync(false);

        // Act
        var result = await _controller.Update(999, dto);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _mockProductService.Verify(s => s.UpdateAsync(999, dto), Times.Once);
    }

    [Fact]
    public async Task Delete_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        _mockProductService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _mockProductService.Verify(s => s.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task Delete_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        _mockProductService.Setup(s => s.DeleteAsync(999)).ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _mockProductService.Verify(s => s.DeleteAsync(999), Times.Once);
    }
}
