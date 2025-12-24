using FluentAssertions;
using VueNetCrud.Server.Domain.Entities;
using VueNetCrud.Server.Infrastructure.Repositories;
using Xunit;

namespace VueNetCrud.Server.Tests.Infrastructure.Repositories;

public class ProductRepositoryTests
{
    private readonly ProductRepository _repository;

    public ProductRepositoryTests()
    {
        _repository = new ProductRepository();
    }

    [Fact]
    public void GetAll_ShouldReturnInitialProducts()
    {
        // Act
        var result = _repository.GetAll().ToList();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThanOrEqualTo(2); // At least initial 2 products
        // Verify initial products exist (may have more from other tests)
        var laptop = result.FirstOrDefault(p => p.Id == 1 && p.Name == "Laptop");
        var mouse = result.FirstOrDefault(p => p.Id == 2 && p.Name == "Mouse");
        laptop.Should().NotBeNull("Initial product 'Laptop' with ID 1 should exist");
        mouse.Should().NotBeNull("Initial product 'Mouse' with ID 2 should exist");
    }

    [Fact]
    public void GetById_WithExistingId_ShouldReturnProduct()
    {
        // Arrange - Ensure product with ID 1 exists and has expected initial values
        // If it was modified by other tests, restore it to initial state
        var existing = _repository.GetById(1);
        if (existing != null && existing.Name != "Laptop")
        {
            // Restore to initial state
            _repository.Update(new Product { Id = 1, Name = "Laptop", Price = 75000, Category = "Electronics" });
        }

        // Act
        var result = _repository.GetById(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Laptop");
        result.Price.Should().Be(75000);
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
    public void Add_ShouldAssignIdAndReturnProduct()
    {
        // Arrange
        var product = new Product { Id = 0, Name = "New Product", Price = 500, Category = "Electronics" };

        // Act
        var result = _repository.Add(product);

        // Assert
        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be("New Product");
    }

    [Fact]
    public void Add_ShouldIncrementId()
    {
        // Arrange
        var product1 = new Product { Id = 0, Name = "Product 1", Price = 100, Category = "Electronics" };
        var product2 = new Product { Id = 0, Name = "Product 2", Price = 200, Category = "Books" };

        // Act
        var result1 = _repository.Add(product1);
        var result2 = _repository.Add(product2);

        // Assert
        result2.Id.Should().BeGreaterThan(result1.Id);
    }

    [Fact]
    public void Update_WithExistingId_ShouldUpdateProduct()
    {
        // Arrange - Create a new product to avoid modifying shared initial data
        var newProduct = new Product { Id = 0, Name = "Test Product", Price = 100, Category = "Electronics" };
        var added = _repository.Add(newProduct);
        var originalName = added.Name;
        
        var product = new Product { Id = added.Id, Name = "Updated Test Product", Price = 80000, Category = "Electronics" };

        // Act
        var result = _repository.Update(product);

        // Assert
        result.Should().BeTrue();
        var updated = _repository.GetById(added.Id);
        updated.Should().NotBeNull();
        updated!.Name.Should().Be("Updated Test Product");
        updated.Price.Should().Be(80000);
        
        // Cleanup - restore original or delete (optional, but good practice)
        _repository.Update(new Product { Id = added.Id, Name = originalName, Price = added.Price, Category = added.Category });
    }

    [Fact]
    public void Update_WithNonExistentId_ShouldReturnFalse()
    {
        // Arrange
        var product = new Product { Id = 999, Name = "Non-existent", Price = 100, Category = "Electronics" };

        // Act
        var result = _repository.Update(product);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Delete_WithExistingId_ShouldReturnTrue()
    {
        // Act
        var result = _repository.Delete(2);

        // Assert
        result.Should().BeTrue();
        _repository.GetById(2).Should().BeNull();
    }

    [Fact]
    public void Delete_WithNonExistentId_ShouldReturnFalse()
    {
        // Act
        var result = _repository.Delete(999);

        // Assert
        result.Should().BeFalse();
    }
}

