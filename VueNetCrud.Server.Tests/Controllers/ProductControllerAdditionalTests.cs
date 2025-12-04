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
    public class ProductControllerAdditionalTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<ILogger<ProductController>> _mockLogger;
        private readonly ProductController _controller;

        public ProductControllerAdditionalTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<ProductController>>();
            _controller = new ProductController(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public void GetAll_WithEmptyList_ShouldReturnOk()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAll()).Returns(Enumerable.Empty<Product>());

            // Act
            var result = _controller.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var products = okResult!.Value as IEnumerable<Product>;
            products.Should().BeEmpty();
        }

        [Fact]
        public void Add_ShouldLogInformation()
        {
            // Arrange
            var product = new Product { Id = 0, Name = "New Product", Price = 100, Category = "Electronics" };
            var createdProduct = new Product { Id = 1, Name = "New Product", Price = 100, Category = "Electronics" };
            _mockRepository.Setup(r => r.Add(product)).Returns(createdProduct);

            // Act
            _controller.Add(product);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        public void Update_ShouldLogInformation()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Updated", Price = 100, Category = "Electronics" };
            _mockRepository.Setup(r => r.Update(product)).Returns(true);

            // Act
            _controller.Update(1, product);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        public void Delete_ShouldLogWarning()
        {
            // Arrange
            _mockRepository.Setup(r => r.Delete(1)).Returns(true);

            // Act
            _controller.Delete(1);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }
    }
}

