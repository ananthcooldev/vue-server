using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Filters;
using VueNetCrud.Server.Application.Validators;
using Xunit;

namespace VueNetCrud.Server.Tests.Application.Filters;

public class ValidationFilterTests
{
    private readonly ValidationFilter _filter;
    private readonly ActionExecutingContext _context;

    public ValidationFilterTests()
    {
        _filter = new ValidationFilter();

        var httpContext = new DefaultHttpContext();
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();
        httpContext.RequestServices = serviceProvider;

        var routeData = new RouteData();
        var actionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor();
        
        var actionContext = new ActionContext
        {
            HttpContext = httpContext,
            RouteData = routeData,
            ActionDescriptor = actionDescriptor
        };

        _context = new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            Mock.Of<Microsoft.AspNetCore.Mvc.Controller>()
        );
    }

    [Fact]
    public void OnActionExecuting_WithValidDto_ShouldNotSetResult()
    {
        // Arrange
        var dto = new ItemCreateDto("Valid Name", "Description");
        var validator = new ItemCreateDtoValidator();
        _context.ActionArguments["dto"] = dto;
        
        // Setup service provider to return validator
        var services = new ServiceCollection();
        services.AddSingleton<IValidator<ItemCreateDto>>(validator);
        var serviceProvider = services.BuildServiceProvider();
        _context.HttpContext.RequestServices = serviceProvider;

        // Act
        _filter.OnActionExecuting(_context);

        // Assert
        _context.Result.Should().BeNull();
    }

    [Fact]
    public void OnActionExecuting_WithInvalidDto_ShouldSetBadRequestResult()
    {
        // Arrange
        var dto = new ItemCreateDto("", "Description");
        var validator = new ItemCreateDtoValidator();
        _context.ActionArguments["dto"] = dto;
        
        // Setup service provider to return validator
        var services = new ServiceCollection();
        services.AddSingleton<IValidator<ItemCreateDto>>(validator);
        var serviceProvider = services.BuildServiceProvider();
        _context.HttpContext.RequestServices = serviceProvider;

        // Act
        _filter.OnActionExecuting(_context);

        // Assert
        _context.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void OnActionExecuting_WithNullArgument_ShouldNotSetResult()
    {
        // Arrange
        _context.ActionArguments["dto"] = null;

        // Act
        _filter.OnActionExecuting(_context);

        // Assert
        _context.Result.Should().BeNull();
    }

    [Fact]
    public void OnActionExecuting_WithNoValidator_ShouldNotSetResult()
    {
        // Arrange
        var dto = new ItemCreateDto("Valid Name", "Description");
        _context.ActionArguments["dto"] = dto;
        
        // Use an empty service provider (no validator registered)
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();
        _context.HttpContext.RequestServices = serviceProvider;

        // Act
        _filter.OnActionExecuting(_context);

        // Assert
        _context.Result.Should().BeNull();
    }

    [Fact]
    public void OnActionExecuted_ShouldNotThrow()
    {
        // Arrange
        var routeData = new RouteData();
        var actionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor();
        
        var actionContext = new ActionContext
        {
            HttpContext = _context.HttpContext,
            RouteData = routeData,
            ActionDescriptor = actionDescriptor
        };
        
        var executedContext = new ActionExecutedContext(
            actionContext,
            new List<IFilterMetadata>(),
            Mock.Of<Microsoft.AspNetCore.Mvc.Controller>()
        );

        // Act & Assert
        _filter.OnActionExecuted(executedContext);
        // Should not throw
    }
}
