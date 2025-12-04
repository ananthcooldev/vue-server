using FluentAssertions;
using VueNetCrud.Server.Models;
using VueNetCrud.Server.Services;
using Xunit;

namespace VueNetCrud.Server.Tests.Services
{
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
        public void Create_WithValidDto_ShouldCreateItem()
        {
            // Arrange
            var dto = new ItemCreate("Test Item", "Test Description");

            // Act
            var result = _repository.Create(dto);

            // Assert
            result.Should().NotBeNull();
            result.id.Should().BeGreaterThan(0);
            result.name.Should().Be("Test Item");
            result.description.Should().Be("Test Description");
        }

        [Fact]
        public void Create_ShouldAutoIncrementId()
        {
            // Arrange
            var dto1 = new ItemCreate("Item 1", "Description 1");
            var dto2 = new ItemCreate("Item 2", "Description 2");

            // Act
            var result1 = _repository.Create(dto1);
            var result2 = _repository.Create(dto2);

            // Assert
            result2.id.Should().BeGreaterThan(result1.id);
        }

        [Fact]
        public void Create_WithEmptyName_ShouldThrowArgumentException()
        {
            // Arrange
            var dto = new ItemCreate("", "Description");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _repository.Create(dto));
        }

        [Fact]
        public void Create_WithWhitespaceName_ShouldThrowArgumentException()
        {
            // Arrange
            var dto = new ItemCreate("   ", "Description");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _repository.Create(dto));
        }

        [Fact]
        public void Create_WithNullName_ShouldThrowArgumentException()
        {
            // Arrange
            var dto = new ItemCreate(null!, "Description");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _repository.Create(dto));
        }

        [Fact]
        public void Create_ShouldTrimNameAndDescription()
        {
            // Arrange
            var dto = new ItemCreate("  Test Item  ", "  Test Description  ");

            // Act
            var result = _repository.Create(dto);

            // Assert
            result.name.Should().Be("Test Item");
            result.description.Should().Be("Test Description");
        }

        [Fact]
        public void Update_WithValidId_ShouldUpdateItem()
        {
            // Arrange
            var createDto = new ItemCreate("Original Name", "Original Description");
            var created = _repository.Create(createDto);
            var updateDto = new ItemUpdate("Updated Name", "Updated Description");

            // Act
            var result = _repository.Update(created.id, updateDto);

            // Assert
            result.Should().NotBeNull();
            result!.name.Should().Be("Updated Name");
            result.description.Should().Be("Updated Description");
        }

        [Fact]
        public void Update_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var updateDto = new ItemUpdate("Updated Name", "Updated Description");

            // Act
            var result = _repository.Update(999, updateDto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Update_WithEmptyName_ShouldKeepOriginalName()
        {
            // Arrange
            var createDto = new ItemCreate("Original Name", "Original Description");
            var created = _repository.Create(createDto);
            var updateDto = new ItemUpdate("", "Updated Description");

            // Act
            var result = _repository.Update(created.id, updateDto);

            // Assert
            result.Should().NotBeNull();
            result!.name.Should().Be("Original Name");
            result.description.Should().Be("Updated Description");
        }

        [Fact]
        public void Update_ShouldTrimValues()
        {
            // Arrange
            var createDto = new ItemCreate("Original Name", "Original Description");
            var created = _repository.Create(createDto);
            var updateDto = new ItemUpdate("  Updated Name  ", "  Updated Description  ");

            // Act
            var result = _repository.Update(created.id, updateDto);

            // Assert
            result.Should().NotBeNull();
            result!.name.Should().Be("Updated Name");
            result.description.Should().Be("Updated Description");
        }

        [Fact]
        public void Delete_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            var createDto = new ItemCreate("To Delete", "Description");
            var created = _repository.Create(createDto);

            // Act
            var result = _repository.Delete(created.id);

            // Assert
            result.Should().BeTrue();
            _repository.GetById(created.id).Should().BeNull();
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
            var dto1 = new ItemCreate("Item 1", "Description 1");
            var dto2 = new ItemCreate("Item 2", "Description 2");
            var created1 = _repository.Create(dto1);
            var created2 = _repository.Create(dto2);

            // Act
            var result = _repository.GetAll();

            // Assert
            result.Should().Contain(i => i.id == created1.id);
            result.Should().Contain(i => i.id == created2.id);
        }
    }
}

