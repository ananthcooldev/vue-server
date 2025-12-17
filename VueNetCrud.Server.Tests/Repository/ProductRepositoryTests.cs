using FluentAssertions;
using VueNetCrud.Server.Domain.Entities;
using VueNetCrud.Server.Infrastructure.Repositories;
using Xunit;

namespace VueNetCrud.Server.Tests.Repository;

public class ProductRepositoryTests
{
    private readonly ProductRepository _repository;

    public ProductRepositoryTests()
    {
        _repository = new ProductRepository();
    }

    [Fact]
    public void GetAll_ShouldReturnAllProducts()
    {
        // Act
        var result = _repository.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public void GetById_WithValidId_ShouldReturnProduct()
    {
        // Arrange - Ensure product with ID 1 exists by checking first
        var allProducts = _repository.GetAll().ToList();
        if (!allProducts.Any(p => p.Id == 1))
        {
            // If product 1 doesn't exist, create it
            _repository.Add(new Product { Id = 0, Name = "Test Product", Price = 100, Category = "Electronics" });
        }

        // Act
        var result = _repository.GetById(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
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
    public void Add_ShouldAddProductAndReturnIt()
    {
        // Arrange
        var product = new Product
        {
            Id = 0,
            Name = "Test Product",
            Price = 1000,
            Category = "Electronics"
        };

        // Act
        var result = _repository.Add(product);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be("Test Product");
        result.Price.Should().Be(1000);
        result.Category.Should().Be("Electronics");
    }

    [Fact]
    public void Add_ShouldAutoIncrementId()
    {
        // Arrange
        var product1 = new Product { Id = 0, Name = "Product 1", Price = 100, Category = "Electronics" };
        var product2 = new Product { Id = 0, Name = "Product 2", Price = 200, Category = "Electronics" };

        // Act
        var result1 = _repository.Add(product1);
        var result2 = _repository.Add(product2);

        // Assert
        result2.Id.Should().BeGreaterThan(result1.Id);
    }

    [Fact]
    public void Update_WithValidId_ShouldUpdateProduct()
    {
        // Arrange - Create a product first to ensure it exists
        var productToUpdate = new Product { Id = 0, Name = "Original Name", Price = 100, Category = "Electronics" };
        var added = _repository.Add(productToUpdate);
        
        var updatedProduct = new Product
        {
            Id = added.Id,
            Name = "Updated Name",
            Price = 99999,
            Category = "Books"
        };

        // Act
        var result = _repository.Update(updatedProduct);

        // Assert
        result.Should().BeTrue();
        var product = _repository.GetById(added.Id);
        product.Should().NotBeNull();
        product!.Name.Should().Be("Updated Name");
        product.Price.Should().Be(99999);
        product.Category.Should().Be("Books");
    }

    [Fact]
    public void Update_WithInvalidId_ShouldReturnFalse()
    {
        // Arrange
        var product = new Product
        {
            Id = 99999,
            Name = "Non-existent",
            Price = 100,
            Category = "Electronics"
        };

        // Act
        var result = _repository.Update(product);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Delete_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        var product = new Product { Id = 0, Name = "To Delete", Price = 100, Category = "Electronics" };
        var added = _repository.Add(product);

        // Act
        var result = _repository.Delete(added.Id);

        // Assert
        result.Should().BeTrue();
        _repository.GetById(added.Id).Should().BeNull();
    }

    [Fact]
    public void Delete_WithInvalidId_ShouldReturnFalse()
    {
        // Act
        var result = _repository.Delete(99999);

        // Assert
        result.Should().BeFalse();
    }
}
