using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VueNetCrud.Server.Controllers;
using VueNetCrud.Server.Models;
using VueNetCrud.Server.Services;
using Xunit;

namespace VueNetCrud.Server.Tests.Controllers
{
    public class ItemsControllerAdditionalTests
    {
        private readonly ItemRepository _repository;
        private readonly Mock<ILogger<ItemsController>> _mockLogger;
        private readonly ItemsController _controller;

        public ItemsControllerAdditionalTests()
        {
            _repository = new ItemRepository();
            _mockLogger = new Mock<ILogger<ItemsController>>();
            _controller = new ItemsController(_repository, _mockLogger.Object);
        }

        [Fact]
        public void GetAll_ShouldLogInformation()
        {
            // Act
            _controller.GetAll();

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
        public void GetById_ShouldLogInformation()
        {
            // Arrange
            var dto = new ItemCreate("Test Item", "Description");
            var created = _repository.Create(dto);

            // Act
            _controller.GetById(created.id);

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
        public void Create_ShouldLogInformation()
        {
            // Arrange
            var dto = new ItemCreate("New Item", "Description");

            // Act
            _controller.Create(dto);

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
            var createDto = new ItemCreate("Original", "Description");
            var created = _repository.Create(createDto);
            var updateDto = new ItemUpdate("Updated", "New Description");

            // Act
            _controller.Update(created.id, updateDto);

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
        public void Delete_ShouldLogInformation()
        {
            // Arrange
            var dto = new ItemCreate("To Delete", "Description");
            var created = _repository.Create(dto);

            // Act
            _controller.Delete(created.id);

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
        public void TestError_ShouldLogInformation()
        {
            // Act & Assert
            Assert.Throws<Exception>(() => _controller.TestError());
            
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }
    }
}

