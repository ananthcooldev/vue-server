using FluentAssertions;
using VueNetCrud.Server.Domain.Entities;
using VueNetCrud.Server.Infrastructure.Repositories;
using Xunit;

namespace VueNetCrud.Server.Tests.Infrastructure.Repositories;

public class ItemRepositoryTests
{
    private readonly ItemRepository _repository;

    public ItemRepositoryTests()
    {
        _repository = new ItemRepository();
    }

    [Fact]
    public void GetAll_Initially_ShouldReturnEmptyList()
    {
        // Act
        var result = _repository.GetAll();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Create_ShouldAddItemAndReturnWithId()
    {
        // Arrange
        var item = new Item(0, "Test Item", "Test Description");

        // Act
        var result = _repository.Create(item);

        // Assert
        result.Id.Should().Be(1);
        result.Name.Should().Be("Test Item");
        result.Description.Should().Be("Test Description");
    }

    [Fact]
    public void Create_MultipleItems_ShouldAssignSequentialIds()
    {
        // Arrange
        var item1 = new Item(0, "Item 1", "Description 1");
        var item2 = new Item(0, "Item 2", "Description 2");

        // Act
        var result1 = _repository.Create(item1);
        var result2 = _repository.Create(item2);

        // Assert
        result1.Id.Should().Be(1);
        result2.Id.Should().Be(2);
    }

    [Fact]
    public void GetById_WithExistingId_ShouldReturnItem()
    {
        // Arrange
        var item = new Item(0, "Test Item", "Test Description");
        var created = _repository.Create(item);

        // Act
        var result = _repository.GetById(created.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(created.Id);
        result.Name.Should().Be("Test Item");
    }

    [Fact]
    public void GetById_WithNonExistentId_ShouldReturnNull()
    {
        // Act
        var result = _repository.GetById(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetAll_AfterCreatingItems_ShouldReturnAllItems()
    {
        // Arrange
        _repository.Create(new Item(0, "Item 1", "Description 1"));
        _repository.Create(new Item(0, "Item 2", "Description 2"));

        // Act
        var result = _repository.GetAll();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public void Update_WithExistingId_ShouldUpdateItem()
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
        
        var retrieved = _repository.GetById(created.Id);
        retrieved!.Name.Should().Be("Updated Name");
    }

    [Fact]
    public void Update_WithNonExistentId_ShouldReturnNull()
    {
        // Arrange
        var updatedItem = new Item(999, "Updated Name", "Updated Description");

        // Act
        var result = _repository.Update(999, updatedItem);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Delete_WithExistingId_ShouldReturnTrue()
    {
        // Arrange
        var item = new Item(0, "Test Item", "Test Description");
        var created = _repository.Create(item);

        // Act
        var result = _repository.Delete(created.Id);

        // Assert
        result.Should().BeTrue();
        _repository.GetById(created.Id).Should().BeNull();
    }

    [Fact]
    public void Delete_WithNonExistentId_ShouldReturnFalse()
    {
        // Act
        var result = _repository.Delete(999);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetAll_AfterDeletingItem_ShouldNotContainDeletedItem()
    {
        // Arrange
        var item1 = _repository.Create(new Item(0, "Item 1", "Description 1"));
        var item2 = _repository.Create(new Item(0, "Item 2", "Description 2"));
        _repository.Delete(item1.Id);

        // Act
        var result = _repository.GetAll();

        // Assert
        result.Should().HaveCount(1);
        result.First().Id.Should().Be(item2.Id);
    }
}

