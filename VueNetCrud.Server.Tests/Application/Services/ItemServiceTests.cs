using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Services;
using VueNetCrud.Server.Domain.Entities;
using VueNetCrud.Server.Domain.Interfaces.Repositories;
using Xunit;

namespace VueNetCrud.Server.Tests.Application.Services;

public class ItemServiceTests
{
    private readonly Mock<IItemRepository> _mockRepository;
    private readonly Mock<ILogger<ItemService>> _mockLogger;
    private readonly ItemService _service;

    public ItemServiceTests()
    {
        _mockRepository = new Mock<IItemRepository>();
        _mockLogger = new Mock<ILogger<ItemService>>();
        _service = new ItemService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllItems()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item(1, "Item 1", "Description 1"),
            new Item(2, "Item 2", "Description 2")
        };
        _mockRepository.Setup(r => r.GetAll()).Returns(items);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.First().Id.Should().Be(1);
        result.First().Name.Should().Be("Item 1");
        _mockRepository.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnItem()
    {
        // Arrange
        var item = new Item(1, "Item 1", "Description 1");
        _mockRepository.Setup(r => r.GetById(1)).Returns(item);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Item 1");
        _mockRepository.Verify(r => r.GetById(1), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetById(999)).Returns((Item?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.GetById(999), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidDto_ShouldReturnCreatedItem()
    {
        // Arrange
        var dto = new ItemCreateDto("New Item", "New Description");
        var createdItem = new Item(1, "New Item", "New Description");
        _mockRepository.Setup(r => r.Create(It.IsAny<Item>())).Returns(createdItem);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Name.Should().Be("New Item");
        _mockRepository.Verify(r => r.Create(It.Is<Item>(i => i.Name == "New Item")), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithEmptyName_ShouldThrowArgumentException()
    {
        // Arrange
        var dto = new ItemCreateDto("", "Description");

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
        _mockRepository.Verify(r => r.Create(It.IsAny<Item>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithWhitespaceName_ShouldThrowArgumentException()
    {
        // Arrange
        var dto = new ItemCreateDto("   ", "Description");

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
        _mockRepository.Verify(r => r.Create(It.IsAny<Item>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WithValidId_ShouldReturnUpdatedItem()
    {
        // Arrange
        var existingItem = new Item(1, "Old Name", "Old Description");
        var updatedItem = new Item(1, "New Name", "New Description");
        var dto = new ItemUpdateDto("New Name", "New Description");

        _mockRepository.Setup(r => r.GetById(1)).Returns(existingItem);
        _mockRepository.Setup(r => r.Update(1, It.IsAny<Item>())).Returns(updatedItem);

        // Act
        var result = await _service.UpdateAsync(1, dto);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("New Name");
        _mockRepository.Verify(r => r.GetById(1), Times.Once);
        _mockRepository.Verify(r => r.Update(1, It.IsAny<Item>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var dto = new ItemUpdateDto("New Name", "New Description");
        _mockRepository.Setup(r => r.GetById(999)).Returns((Item?)null);

        // Act
        var result = await _service.UpdateAsync(999, dto);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.GetById(999), Times.Once);
        _mockRepository.Verify(r => r.Update(It.IsAny<int>(), It.IsAny<Item>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WithEmptyName_ShouldKeepExistingName()
    {
        // Arrange
        var existingItem = new Item(1, "Existing Name", "Description");
        var dto = new ItemUpdateDto("", "New Description");
        _mockRepository.Setup(r => r.GetById(1)).Returns(existingItem);
        _mockRepository.Setup(r => r.Update(1, It.Is<Item>(i => i.Name == "Existing Name"))).Returns(existingItem);

        // Act
        var result = await _service.UpdateAsync(1, dto);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Existing Name");
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

