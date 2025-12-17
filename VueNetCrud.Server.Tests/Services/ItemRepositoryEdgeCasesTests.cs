using FluentAssertions;
using VueNetCrud.Server.Domain.Entities;
using VueNetCrud.Server.Infrastructure.Repositories;
using Xunit;

namespace VueNetCrud.Server.Tests.Services;

public class ItemRepositoryEdgeCasesTests
{
    [Fact]
    public void Create_WithOnlyWhitespaceInName_ShouldCreate()
    {
        // Arrange
        var repository = new ItemRepository();
        var item = new Item(0, "   \t\n   ", "Description");

        // Act
        var result = repository.Create(item);

        // Assert
        result.Should().NotBeNull();
        // Note: Repository doesn't validate, service layer does
    }

    [Fact]
    public void Create_WithTabAndNewlineInName_ShouldCreate()
    {
        // Arrange
        var repository = new ItemRepository();
        var item = new Item(0, "\t\n", "Description");

        // Act
        var result = repository.Create(item);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void Update_WithOnlyWhitespaceInName_ShouldUpdate()
    {
        // Arrange
        var repository = new ItemRepository();
        var item = new Item(0, "Original Name", "Description");
        var created = repository.Create(item);
        var updatedItem = new Item(created.Id, "   \t\n   ", "Updated Description");

        // Act
        var result = repository.Update(created.Id, updatedItem);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("   \t\n   ");
    }

    [Fact]
    public void Update_WithWhitespaceDescription_ShouldUpdate()
    {
        // Arrange
        var repository = new ItemRepository();
        var item = new Item(0, "Item", "Original Description");
        var created = repository.Create(item);
        var updatedItem = new Item(created.Id, "Updated Name", "   Trimmed Description   ");

        // Act
        var result = repository.Update(created.Id, updatedItem);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Updated Name");
        result.Description.Should().Be("   Trimmed Description   ");
    }

    [Fact]
    public void Update_WithNullDescription_ShouldSetDescriptionToNull()
    {
        // Arrange
        var repository = new ItemRepository();
        var item = new Item(0, "Item", "Original Description");
        var created = repository.Create(item);
        var updatedItem = new Item(created.Id, "Updated Name", null);

        // Act
        var result = repository.Update(created.Id, updatedItem);

        // Assert
        result.Should().NotBeNull();
        result!.Description.Should().BeNull();
    }

    [Fact]
    public void GetById_WithZeroId_ShouldReturnNull()
    {
        // Arrange
        var repository = new ItemRepository();

        // Act
        var result = repository.GetById(0);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetById_WithNegativeId_ShouldReturnNull()
    {
        // Arrange
        var repository = new ItemRepository();

        // Act
        var result = repository.GetById(-1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Delete_WithZeroId_ShouldReturnFalse()
    {
        // Arrange
        var repository = new ItemRepository();

        // Act
        var result = repository.Delete(0);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Delete_WithNegativeId_ShouldReturnFalse()
    {
        // Arrange
        var repository = new ItemRepository();

        // Act
        var result = repository.Delete(-1);

        // Assert
        result.Should().BeFalse();
    }
}
