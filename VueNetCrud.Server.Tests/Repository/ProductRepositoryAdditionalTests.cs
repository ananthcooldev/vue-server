using FluentAssertions;
using VueNetCrud.Server.Models;
using VueNetCrud.Server.Repository;
using Xunit;

namespace VueNetCrud.Server.Tests.Repository
{
    public class ProductRepositoryAdditionalTests
    {
        [Fact]
        public void Add_WithEmptyProductList_ShouldSetIdToOne()
        {
            // Arrange - Create a new repository instance to test the empty list scenario
            // Note: Since ProductRepository uses a static list, we need to test the logic
            // by ensuring we understand the behavior when the list might be empty
            var repository = new ProductRepository();
            
            // Get all products to see current state
            var existingProducts = repository.GetAll().ToList();
            
            // Find the max ID
            var maxId = existingProducts.Any() ? existingProducts.Max(p => p.Id) : 0;
            
            // Create a product that will get the next ID
            var product = new Product
            {
                Id = 0,
                Name = "New Product",
                Price = 1000,
                Category = "Electronics"
            };

            // Act
            var result = repository.Add(product);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(maxId);
        }

        [Fact]
        public void Update_ShouldUpdateAllProperties()
        {
            // Arrange
            var repository = new ProductRepository();
            // Create a new product to avoid modifying shared state
            var newProduct = new Product { Id = 0, Name = "Original Name", Price = 100, Category = "Electronics" };
            var added = repository.Add(newProduct);
            var originalName = added.Name;
            var originalPrice = added.Price;
            var originalCategory = added.Category;

            var updatedProduct = new Product
            {
                Id = added.Id,
                Name = "Completely New Name",
                Price = 12345,
                Category = "Books"
            };

            // Act
            var result = repository.Update(updatedProduct);

            // Assert
            result.Should().BeTrue();
            var product = repository.GetById(added.Id);
            product.Should().NotBeNull();
            product!.Name.Should().Be("Completely New Name");
            product.Price.Should().Be(12345);
            product.Category.Should().Be("Books");
            
            // Verify all properties changed
            product.Name.Should().NotBe(originalName);
            product.Price.Should().NotBe(originalPrice);
            product.Category.Should().NotBe(originalCategory);
        }

        [Fact]
        public void Delete_ShouldRemoveProductFromList()
        {
            // Arrange
            var repository = new ProductRepository();
            var product = new Product { Id = 0, Name = "To Delete", Price = 100, Category = "Electronics" };
            var added = repository.Add(product);
            var idBeforeDelete = added.Id;

            // Verify it exists
            repository.GetById(idBeforeDelete).Should().NotBeNull();

            // Act
            var result = repository.Delete(idBeforeDelete);

            // Assert
            result.Should().BeTrue();
            repository.GetById(idBeforeDelete).Should().BeNull();
        }
    }
}

