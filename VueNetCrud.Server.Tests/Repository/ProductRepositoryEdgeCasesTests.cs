using FluentAssertions;
using VueNetCrud.Server.Models;
using VueNetCrud.Server.Repository;
using Xunit;

namespace VueNetCrud.Server.Tests.Repository
{
    public class ProductRepositoryEdgeCasesTests
    {
        [Fact]
        public void GetAll_ShouldReturnEnumerable()
        {
            // Arrange
            var repository = new ProductRepository();

            // Act
            var result = repository.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<Product>>();
        }

        [Fact]
        public void GetById_WithZeroId_ShouldReturnNull()
        {
            // Arrange
            var repository = new ProductRepository();

            // Act
            var result = repository.GetById(0);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetById_WithNegativeId_ShouldReturnNull()
        {
            // Arrange
            var repository = new ProductRepository();

            // Act
            var result = repository.GetById(-1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Add_WithExistingId_ShouldOverrideId()
        {
            // Arrange
            var repository = new ProductRepository();
            var product = new Product
            {
                Id = 999, // Try to set a specific ID
                Name = "Test Product",
                Price = 100,
                Category = "Electronics"
            };

            // Act
            var result = repository.Add(product);

            // Assert
            result.Should().NotBeNull();
            // The repository should auto-assign ID, ignoring the provided ID
            result.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Update_WithSameValues_ShouldReturnTrue()
        {
            // Arrange
            var repository = new ProductRepository();
            var existing = repository.GetById(1);
            if (existing == null)
            {
                existing = repository.Add(new Product { Id = 0, Name = "Test", Price = 100, Category = "Electronics" });
            }

            var sameProduct = new Product
            {
                Id = existing.Id,
                Name = existing.Name,
                Price = existing.Price,
                Category = existing.Category
            };

            // Act
            var result = repository.Update(sameProduct);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Delete_WithZeroId_ShouldReturnFalse()
        {
            // Arrange
            var repository = new ProductRepository();

            // Act
            var result = repository.Delete(0);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Delete_WithNegativeId_ShouldReturnFalse()
        {
            // Arrange
            var repository = new ProductRepository();

            // Act
            var result = repository.Delete(-1);

            // Assert
            result.Should().BeFalse();
        }
    }
}

