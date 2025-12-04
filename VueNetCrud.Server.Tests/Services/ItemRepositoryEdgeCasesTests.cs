using FluentAssertions;
using VueNetCrud.Server.Models;
using VueNetCrud.Server.Services;
using Xunit;

namespace VueNetCrud.Server.Tests.Services
{
    public class ItemRepositoryEdgeCasesTests
    {
        [Fact]
        public void Create_WithOnlyWhitespaceInName_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new ItemRepository();
            var dto = new ItemCreate("   \t\n   ", "Description");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Create(dto));
        }

        [Fact]
        public void Create_WithTabAndNewlineInName_ShouldThrowArgumentException()
        {
            // Arrange
            var repository = new ItemRepository();
            var dto = new ItemCreate("\t\n", "Description");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => repository.Create(dto));
        }

        [Fact]
        public void Update_WithOnlyWhitespaceInName_ShouldKeepOriginalName()
        {
            // Arrange
            var repository = new ItemRepository();
            var createDto = new ItemCreate("Original Name", "Description");
            var created = repository.Create(createDto);
            var updateDto = new ItemUpdate("   \t\n   ", "Updated Description");

            // Act
            var result = repository.Update(created.id, updateDto);

            // Assert
            result.Should().NotBeNull();
            result!.name.Should().Be("Original Name");
            result.description.Should().Be("Updated Description");
        }

        [Fact]
        public void Update_WithWhitespaceDescription_ShouldTrimDescription()
        {
            // Arrange
            var repository = new ItemRepository();
            var createDto = new ItemCreate("Item", "Original Description");
            var created = repository.Create(createDto);
            var updateDto = new ItemUpdate("Updated Name", "   Trimmed Description   ");

            // Act
            var result = repository.Update(created.id, updateDto);

            // Assert
            result.Should().NotBeNull();
            result!.name.Should().Be("Updated Name");
            result.description.Should().Be("Trimmed Description");
        }

        [Fact]
        public void Update_WithNullDescription_ShouldSetDescriptionToNull()
        {
            // Arrange
            var repository = new ItemRepository();
            var createDto = new ItemCreate("Item", "Original Description");
            var created = repository.Create(createDto);
            var updateDto = new ItemUpdate("Updated Name", null);

            // Act
            var result = repository.Update(created.id, updateDto);

            // Assert
            result.Should().NotBeNull();
            result!.description.Should().BeNull();
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
}

