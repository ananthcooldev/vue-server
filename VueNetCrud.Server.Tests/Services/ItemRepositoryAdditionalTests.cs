using FluentAssertions;
using VueNetCrud.Server.Models;
using VueNetCrud.Server.Services;
using Xunit;

namespace VueNetCrud.Server.Tests.Services
{
    public class ItemRepositoryAdditionalTests
    {
        [Fact]
        public void Update_WithNullDescription_ShouldSetDescriptionToNull()
        {
            // Arrange
            var repository = new ItemRepository();
            var createDto = new ItemCreate("Original Name", "Original Description");
            var created = repository.Create(createDto);
            var updateDto = new ItemUpdate("Updated Name", null);

            // Act
            var result = repository.Update(created.id, updateDto);

            // Assert
            result.Should().NotBeNull();
            result!.name.Should().Be("Updated Name");
            result.description.Should().BeNull();
        }

        [Fact]
        public void Update_WithWhitespaceName_ShouldKeepOriginalName()
        {
            // Arrange
            var repository = new ItemRepository();
            var createDto = new ItemCreate("Original Name", "Description");
            var created = repository.Create(createDto);
            var updateDto = new ItemUpdate("   ", "Updated Description");

            // Act
            var result = repository.Update(created.id, updateDto);

            // Assert
            result.Should().NotBeNull();
            result!.name.Should().Be("Original Name");
            result.description.Should().Be("Updated Description");
        }

        [Fact]
        public void Update_WithNullName_ShouldKeepOriginalName()
        {
            // Arrange
            var repository = new ItemRepository();
            var createDto = new ItemCreate("Original Name", "Description");
            var created = repository.Create(createDto);
            var updateDto = new ItemUpdate(null, "Updated Description");

            // Act
            var result = repository.Update(created.id, updateDto);

            // Assert
            result.Should().NotBeNull();
            result!.name.Should().Be("Original Name");
            result.description.Should().Be("Updated Description");
        }

        [Fact]
        public void Create_WithNullDescription_ShouldCreateWithNullDescription()
        {
            // Arrange
            var repository = new ItemRepository();
            var dto = new ItemCreate("Test Item", null);

            // Act
            var result = repository.Create(dto);

            // Assert
            result.Should().NotBeNull();
            result.name.Should().Be("Test Item");
            result.description.Should().BeNull();
        }

        [Fact]
        public void GetAll_ShouldReturnReadOnlyList()
        {
            // Arrange
            var repository = new ItemRepository();
            var dto = new ItemCreate("Item 1", "Description 1");
            repository.Create(dto);

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
            var dto1 = new ItemCreate("Item 1", "Description 1");
            var dto2 = new ItemCreate("Item 2", "Description 2");
            var created1 = repository.Create(dto1);
            var created2 = repository.Create(dto2);

            // Act
            var result = repository.Delete(created1.id);

            // Assert
            result.Should().BeTrue();
            repository.GetById(created1.id).Should().BeNull();
            repository.GetById(created2.id).Should().NotBeNull();
        }
    }
}

