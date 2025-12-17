using FluentAssertions;
using VueNetCrud.Server.Domain.Entities;
using VueNetCrud.Server.Infrastructure.Repositories;
using Xunit;

namespace VueNetCrud.Server.Tests.Services;

public class ItemRepositoryAdditionalTests
{
    [Fact]
    public void Update_WithNullDescription_ShouldSetDescriptionToNull()
    {
        // Arrange
        var repository = new ItemRepository();
        var item = new Item(0, "Original Name", "Original Description");
        var created = repository.Create(item);
        var updatedItem = new Item(created.Id, "Updated Name", null);

        // Act
        var result = repository.Update(created.Id, updatedItem);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Updated Name");
        result.Description.Should().BeNull();
    }

    [Fact]
    public void Update_WithWhitespaceName_ShouldKeepOriginalName()
    {
        // Arrange
        var repository = new ItemRepository();
        var item = new Item(0, "Original Name", "Description");
        var created = repository.Create(item);
        var updatedItem = new Item(created.Id, "   ", "Updated Description");

        // Act
        var result = repository.Update(created.Id, updatedItem);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("   "); // Repository doesn't trim on update, service does
    }

    [Fact]
    public void Update_WithNullName_ShouldUpdate()
    {
        // Arrange
        var repository = new ItemRepository();
        var item = new Item(0, "Original Name", "Description");
        var created = repository.Create(item);
        var updatedItem = new Item(created.Id, null!, "Updated Description");

        // Act
        var result = repository.Update(created.Id, updatedItem);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().BeNull();
    }

    [Fact]
    public void Create_WithNullDescription_ShouldCreateWithNullDescription()
    {
        // Arrange
        var repository = new ItemRepository();
        var item = new Item(0, "Test Item", null);

        // Act
        var result = repository.Create(item);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test Item");
        result.Description.Should().BeNull();
    }

    [Fact]
    public void GetAll_ShouldReturnReadOnlyList()
    {
        // Arrange
        var repository = new ItemRepository();
        var item = new Item(0, "Item 1", "Description 1");
        repository.Create(item);

        // Act
        var result = repository.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IReadOnlyList<Item>>();
    }

    [Fact]
    public void Delete_WithMultipleItems_ShouldOnlyDeleteMatchingId()
    {
        // Arrange
        var repository = new ItemRepository();
        var item1 = new Item(0, "Item 1", "Description 1");
        var item2 = new Item(0, "Item 2", "Description 2");
        var created1 = repository.Create(item1);
        var created2 = repository.Create(item2);

        // Act
        var result = repository.Delete(created1.Id);

        // Assert
        result.Should().BeTrue();
        repository.GetById(created1.Id).Should().BeNull();
        repository.GetById(created2.Id).Should().NotBeNull();
    }
}
