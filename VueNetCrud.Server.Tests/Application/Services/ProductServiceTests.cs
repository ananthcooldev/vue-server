using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Services;
using VueNetCrud.Server.Domain.Entities;
using VueNetCrud.Server.Domain.Interfaces.Repositories;
using Xunit;

namespace VueNetCrud.Server.Tests.Application.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly Mock<ILogger<ProductService>> _mockLogger;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _mockRepository = new Mock<IProductRepository>();
        _mockLogger = new Mock<ILogger<ProductService>>();
        _service = new ProductService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Price = 100, Category = "Electronics" },
            new Product { Id = 2, Name = "Product 2", Price = 200, Category = "Books" }
        };
        _mockRepository.Setup(r => r.GetAll()).Returns(products);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.First().Id.Should().Be(1);
        result.First().Name.Should().Be("Product 1");
        _mockRepository.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Product 1", Price = 100, Category = "Electronics" };
        _mockRepository.Setup(r => r.GetById(1)).Returns(product);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Product 1");
        _mockRepository.Verify(r => r.GetById(1), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetById(999)).Returns((Product?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.GetById(999), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidDto_ShouldReturnCreatedProduct()
    {
        // Arrange
        var dto = new ProductCreateDto("New Product", 150, "Electronics");
        var createdProduct = new Product { Id = 1, Name = "New Product", Price = 150, Category = "Electronics" };
        _mockRepository.Setup(r => r.Add(It.IsAny<Product>())).Returns(createdProduct);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Name.Should().Be("New Product");
        result.Price.Should().Be(150);
        _mockRepository.Verify(r => r.Add(It.Is<Product>(p => p.Name == "New Product")), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        var dto = new ProductUpdateDto(1, "Updated Product", 200, "Books");
        _mockRepository.Setup(r => r.Update(It.IsAny<Product>())).Returns(true);

        // Act
        var result = await _service.UpdateAsync(1, dto);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.Update(It.Is<Product>(p => p.Id == 1 && p.Name == "Updated Product")), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithMismatchedId_ShouldReturnFalse()
    {
        // Arrange
        var dto = new ProductUpdateDto(2, "Updated Product", 200, "Books");

        // Act
        var result = await _service.UpdateAsync(1, dto);

        // Assert
        result.Should().BeFalse();
        _mockRepository.Verify(r => r.Update(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ShouldReturnFalse()
    {
        // Arrange
        var dto = new ProductUpdateDto(999, "Updated Product", 200, "Books");
        _mockRepository.Setup(r => r.Update(It.IsAny<Product>())).Returns(false);

        // Act
        var result = await _service.UpdateAsync(999, dto);

        // Assert
        result.Should().BeFalse();
        _mockRepository.Verify(r => r.Update(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        _mockRepository.Setup(r => r.Delete(1)).Returns(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.Delete(1), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ShouldReturnFalse()
    {
        // Arrange
        _mockRepository.Setup(r => r.Delete(999)).Returns(false);

        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        result.Should().BeFalse();
        _mockRepository.Verify(r => r.Delete(999), Times.Once);
    }
}

