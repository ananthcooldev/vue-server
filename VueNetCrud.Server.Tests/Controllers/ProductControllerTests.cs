using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VueNetCrud.Server.Controllers;
using VueNetCrud.Server.Models;
using VueNetCrud.Server.Repository;
using Xunit;

namespace VueNetCrud.Server.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<ILogger<ProductController>> _mockLogger;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<ProductController>>();
            _controller = new ProductController(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnOkWithProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 75000, Category = "Electronics" },
                new Product { Id = 2, Name = "Mouse", Price = 500, Category = "Electronics" }
            };
            _mockRepository.Setup(r => r.GetAll()).Returns(products);

            // Act
            var result = _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(products);
        }

        [Fact]
        public void GetById_WithValidId_ShouldReturnOkWithProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Laptop", Price = 75000, Category = "Electronics" };
            _mockRepository.Setup(r => r.GetById(1)).Returns(product);

            // Act
            var result = _controller.GetById(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(product);
        }

        [Fact]
        public void GetById_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetById(999)).Returns((Product?)null);

            // Act
            var result = _controller.GetById(999);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Add_WithValidProduct_ShouldReturnCreated()
        {
            // Arrange
            var product = new Product { Id = 0, Name = "Keyboard", Price = 1000, Category = "Electronics" };
            var createdProduct = new Product { Id = 3, Name = "Keyboard", Price = 1000, Category = "Electronics" };
            _mockRepository.Setup(r => r.Add(product)).Returns(createdProduct);

            // Act
            var result = _controller.Add(product);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result as CreatedAtActionResult;
            createdResult!.Value.Should().BeEquivalentTo(createdProduct);
            createdResult.ActionName.Should().Be(nameof(ProductController.GetById));
        }

        [Fact]
        public void Update_WithValidProduct_ShouldReturnNoContent()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Updated Laptop", Price = 80000, Category = "Electronics" };
            _mockRepository.Setup(r => r.Update(product)).Returns(true);

            // Act
            var result = _controller.Update(1, product);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void Update_WithMismatchedId_ShouldReturnBadRequest()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Updated Laptop", Price = 80000, Category = "Electronics" };

            // Act
            var result = _controller.Update(2, product);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void Update_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var product = new Product { Id = 999, Name = "Non-existent", Price = 100, Category = "Electronics" };
            _mockRepository.Setup(r => r.Update(product)).Returns(false);

            // Act
            var result = _controller.Update(999, product);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Delete_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            _mockRepository.Setup(r => r.Delete(1)).Returns(true);

            // Act
            var result = _controller.Delete(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void Delete_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            _mockRepository.Setup(r => r.Delete(999)).Returns(false);

            // Act
            var result = _controller.Delete(999);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}

