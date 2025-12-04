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
    public class ValidationFilterAdditionalTests
    {
        [Fact]
        public void OnActionExecuting_WithMultipleArguments_ShouldValidateAll()
        {
            // Arrange
            var filter = new ValidationFilter();
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.RequestServices).Returns(mockServiceProvider.Object);

            var routeData = new RouteData();
            var actionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor();

            var actionContext = new ActionContext
            {
                HttpContext = mockHttpContext.Object,
                RouteData = routeData,
                ActionDescriptor = actionDescriptor
            };

            var executingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>(),
                Mock.Of<Microsoft.AspNetCore.Mvc.ControllerBase>());

            var product1 = new Product { Id = 1, Name = "Product 1", Price = 100, Category = "Electronics" };
            var product2 = new Product { Id = 2, Name = "", Price = -10, Category = "Electronics" }; // Invalid

            var validator = new ProductValidator();
            mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IValidator<>))))
                .Returns(validator);

            executingContext.ActionArguments["product1"] = product1;
            executingContext.ActionArguments["product2"] = product2;

            // Act
            filter.OnActionExecuting(executingContext);

            // Assert
            executingContext.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void OnActionExecuting_WithMultipleArguments_FirstInvalid_ShouldReturnEarly()
        {
            // Arrange
            var filter = new ValidationFilter();
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.RequestServices).Returns(mockServiceProvider.Object);

            var routeData = new RouteData();
            var actionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor();

            var actionContext = new ActionContext
            {
                HttpContext = mockHttpContext.Object,
                RouteData = routeData,
                ActionDescriptor = actionDescriptor
            };

            var executingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>(),
                Mock.Of<Microsoft.AspNetCore.Mvc.ControllerBase>());

            var invalidProduct = new Product { Id = 1, Name = "", Price = -10, Category = "Electronics" };
            var validProduct = new Product { Id = 2, Name = "Valid", Price = 100, Category = "Electronics" };

            var validator = new ProductValidator();
            mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IValidator<>))))
                .Returns(validator);

            executingContext.ActionArguments["invalid"] = invalidProduct;
            executingContext.ActionArguments["valid"] = validProduct;

            // Act
            filter.OnActionExecuting(executingContext);

            // Assert
            executingContext.Result.Should().BeOfType<BadRequestObjectResult>();
            // Verify it returned early and didn't process the second argument
        }

        [Fact]
        public void OnActionExecuting_WithNonValidatableArgument_ShouldNotSetResult()
        {
            // Arrange
            var filter = new ValidationFilter();
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.RequestServices).Returns(mockServiceProvider.Object);

            var routeData = new RouteData();
            var actionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor();

            var actionContext = new ActionContext
            {
                HttpContext = mockHttpContext.Object,
                RouteData = routeData,
                ActionDescriptor = actionDescriptor
            };

            var executingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>(),
                Mock.Of<Microsoft.AspNetCore.Mvc.ControllerBase>());

            var stringArgument = "Not a validatable type";
            mockServiceProvider
                .Setup(x => x.GetService(It.Is<Type>(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IValidator<>))))
                .Returns((object?)null);

            executingContext.ActionArguments["stringArg"] = stringArgument;

            // Act
            filter.OnActionExecuting(executingContext);

            // Assert
            executingContext.Result.Should().BeNull();
        }
    }
}

