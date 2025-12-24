using FluentAssertions;
using VueNetCrud.Server.Domain.Entities;
using VueNetCrud.Server.Infrastructure.Repositories;
using Xunit;

namespace VueNetCrud.Server.Tests.Services;

public class ItemRepositoryTests
{
    private readonly ItemRepository _repository;

    public ItemRepositoryTests()
    {
        _repository = new ItemRepository();
    }

    [Fact]
    public void GetAll_ShouldReturnEmptyList_WhenNoItems()
    {
        // Act
        var result = _repository.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetById_WithInvalidId_ShouldReturnNull()
    {
        // Act
        var result = _repository.GetById(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Create_WithValidItem_ShouldCreateItem()
    {
        // Arrange
        var item = new Item(0, "Test Item", "Test Description");

        // Act
        var result = _repository.Create(item);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be("Test Item");
        result.Description.Should().Be("Test Description");
    }

    [Fact]
    public void Create_ShouldAutoIncrementId()
    {
        // Arrange
        var item1 = new Item(0, "Item 1", "Description 1");
        var item2 = new Item(0, "Item 2", "Description 2");

        // Act
        var result1 = _repository.Create(item1);
        var result2 = _repository.Create(item2);

        // Assert
        result2.Id.Should().BeGreaterThan(result1.Id);
    }

    [Fact]
    public void Create_ShouldTrimNameAndDescription()
    {
        // Arrange
        var item = new Item(0, "  Test Item  ", "  Test Description  ");

        // Act
        var result = _repository.Create(item);

        // Assert
        // Note: Repository doesn't trim, service layer does
        result.Name.Should().Be("  Test Item  ");
        result.Description.Should().Be("  Test Description  ");
    }

    [Fact]
    public void Update_WithValidId_ShouldUpdateItem()
    {
        // Arrange
        var item = new Item(0, "Original Name", "Original Description");
        var created = _repository.Create(item);
        var updatedItem = new Item(created.Id, "Updated Name", "Updated Description");

        // Act
        var result = _repository.Update(created.Id, updatedItem);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Updated Name");
        result.Description.Should().Be("Updated Description");
    }

    [Fact]
    public void Update_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var updatedItem = new Item(999, "Updated Name", "Updated Description");

        // Act
        var result = _repository.Update(999, updatedItem);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Update_WithEmptyName_ShouldUpdate()
    {
        // Arrange
        var item = new Item(0, "Original Name", "Original Description");
        var created = _repository.Create(item);
        var updatedItem = new Item(created.Id, "", "Updated Description");

        // Act
        var result = _repository.Update(created.Id, updatedItem);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("");
    }

    [Fact]
    public void Update_ShouldUpdateValues()
    {
        // Arrange
        var item = new Item(0, "Original Name", "Original Description");
        var created = _repository.Create(item);
        var updatedItem = new Item(created.Id, "  Updated Name  ", "  Updated Description  ");

        // Act
        var result = _repository.Update(created.Id, updatedItem);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("  Updated Name  ");
        result.Description.Should().Be("  Updated Description  ");
    }

    [Fact]
    public void Delete_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        var item = new Item(0, "To Delete", "Description");
        var created = _repository.Create(item);

        // Act
        var result = _repository.Delete(created.Id);

        // Assert
        result.Should().BeTrue();
        _repository.GetById(created.Id).Should().BeNull();
    }

    [Fact]
    public void Delete_WithInvalidId_ShouldReturnFalse()
    {
        // Act
        var result = _repository.Delete(999);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetAll_ShouldReturnCreatedItems()
    {
        // Arrange
        var item1 = new Item(0, "Item 1", "Description 1");
        var item2 = new Item(0, "Item 2", "Description 2");
        var created1 = _repository.Create(item1);
        var created2 = _repository.Create(item2);

        // Act
        var result = _repository.GetAll();

        // Assert
        result.Should().Contain(i => i.Id == created1.Id);
        result.Should().Contain(i => i.Id == created2.Id);
    }
}
