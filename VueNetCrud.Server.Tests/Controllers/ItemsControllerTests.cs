using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VueNetCrud.Server.Controllers;
using VueNetCrud.Server.Models;
using VueNetCrud.Server.Services;
using Xunit;

namespace VueNetCrud.Server.Tests.Controllers
{
    public class ItemsControllerTests
    {
        private readonly ItemRepository _repository;
        private readonly Mock<ILogger<ItemsController>> _mockLogger;
        private readonly ItemsController _controller;

        public ItemsControllerTests()
        {
            _repository = new ItemRepository();
            _mockLogger = new Mock<ILogger<ItemsController>>();
            _controller = new ItemsController(_repository, _mockLogger.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnOkWithItems()
        {
            // Arrange
            var dto1 = new ItemCreate("Item 1", "Description 1");
            var dto2 = new ItemCreate("Item 2", "Description 2");
            var item1 = _repository.Create(dto1);
            var item2 = _repository.Create(dto2);

            // Act
            var result = _controller.GetAll();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var items = okResult!.Value as IEnumerable<Item>;
            items.Should().Contain(i => i.id == item1.id);
            items.Should().Contain(i => i.id == item2.id);
        }

        [Fact]
        public void GetById_WithValidId_ShouldReturnOkWithItem()
        {
            // Arrange
            var dto = new ItemCreate("Item 1", "Description 1");
            var createdItem = _repository.Create(dto);

            // Act
            var result = _controller.GetById(createdItem.id);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var item = okResult!.Value as Item;
            item.Should().BeEquivalentTo(createdItem);
        }

        [Fact]
        public void GetById_WithInvalidId_ShouldReturnNotFound()
        {
            // Act
            var result = _controller.GetById(999);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Create_WithValidDto_ShouldReturnCreated()
        {
            // Arrange
            var dto = new ItemCreate("New Item", "New Description");

            // Act
            var result = _controller.Create(dto);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result.Result as CreatedAtActionResult;
            var createdItem = createdResult!.Value as Item;
            createdItem.Should().NotBeNull();
            createdItem!.name.Should().Be("New Item");
            createdItem.description.Should().Be("New Description");
            createdResult.ActionName.Should().Be(nameof(ItemsController.GetById));
        }

        [Fact]
        public void Create_WithInvalidDto_ShouldReturnBadRequest()
        {
            // Arrange
            var dto = new ItemCreate("", "Description");

            // Act
            var result = _controller.Create(dto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult!.Value.Should().NotBeNull();
        }

        [Fact]
        public void Update_WithValidId_ShouldReturnOk()
        {
            // Arrange
            var createDto = new ItemCreate("Original Item", "Original Description");
            var created = _repository.Create(createDto);
            var updateDto = new ItemUpdate("Updated Item", "Updated Description");

            // Act
            var result = _controller.Update(created.id, updateDto);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var updatedItem = okResult!.Value as Item;
            updatedItem.Should().NotBeNull();
            updatedItem!.name.Should().Be("Updated Item");
            updatedItem.description.Should().Be("Updated Description");
        }

        [Fact]
        public void Update_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var dto = new ItemUpdate("Updated Item", "Updated Description");

            // Act
            var result = _controller.Update(999, dto);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Delete_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            var dto = new ItemCreate("To Delete", "Description");
            var created = _repository.Create(dto);

            // Act
            var result = _controller.Delete(created.id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void Delete_WithInvalidId_ShouldReturnNotFound()
        {
            // Act
            var result = _controller.Delete(999);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void TestError_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<Exception>(() => _controller.TestError());
        }
    }
}

