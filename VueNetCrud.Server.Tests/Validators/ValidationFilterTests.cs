using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using VueNetCrud.Server.Models;
using VueNetCrud.Server.Validators;
using Xunit;

namespace VueNetCrud.Server.Tests.Validators
{
    public class ValidationFilterTests
    {
        private readonly ValidationFilter _filter;
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly ActionExecutingContext _executingContext;
        private readonly ActionExecutedContext _executedContext;

        public ValidationFilterTests()
        {
            _filter = new ValidationFilter();
            _mockServiceProvider = new Mock<IServiceProvider>();
            _mockHttpContext = new Mock<HttpContext>();
            _mockHttpContext.Setup(x => x.RequestServices).Returns(_mockServiceProvider.Object);

            var routeData = new RouteData();
            var actionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor();

            var actionContext = new ActionContext
            {
                HttpContext = _mockHttpContext.Object,
                RouteData = routeData,
                ActionDescriptor = actionDescriptor
            };

            _executingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>(),
                Mock.Of<Microsoft.AspNetCore.Mvc.ControllerBase>());

            _executedContext = new ActionExecutedContext(
                actionContext,
                new List<IFilterMetadata>(),
                Mock.Of<Microsoft.AspNetCore.Mvc.ControllerBase>());
        }

        [Fact]
        public void OnActionExecuting_WithValidProduct_ShouldNotSetResult()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Valid Product",
                Price = 100,
                Category = "Electronics"
            };

            var validator = new ProductValidator();
            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IValidator<>))))
                .Returns(validator);

            _executingContext.ActionArguments["product"] = product;

            // Act
            _filter.OnActionExecuting(_executingContext);

            // Assert
            _executingContext.Result.Should().BeNull();
        }

        [Fact]
        public void OnActionExecuting_WithInvalidProduct_ShouldSetBadRequestResult()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "", // Invalid: empty name
                Price = -10, // Invalid: negative price
                Category = "Electronics"
            };

            var validator = new ProductValidator();
            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IValidator<>))))
                .Returns(validator);

            _executingContext.ActionArguments["product"] = product;

            // Act
            _filter.OnActionExecuting(_executingContext);

            // Assert
            _executingContext.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void OnActionExecuting_WithNullArgument_ShouldNotSetResult()
        {
            // Arrange
            _executingContext.ActionArguments["product"] = null;

            // Act
            _filter.OnActionExecuting(_executingContext);

            // Assert
            _executingContext.Result.Should().BeNull();
        }

        [Fact]
        public void OnActionExecuting_WithNoValidator_ShouldNotSetResult()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product",
                Price = 100,
                Category = "Electronics"
            };

            _mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IValidator<>))))
                .Returns((object?)null);

            _executingContext.ActionArguments["product"] = product;

            // Act
            _filter.OnActionExecuting(_executingContext);

            // Assert
            _executingContext.Result.Should().BeNull();
        }

        [Fact]
        public void OnActionExecuted_ShouldNotThrow()
        {
            // Act
            var act = () => _filter.OnActionExecuted(_executedContext);

            // Assert
            act.Should().NotThrow();
        }
    }
}

