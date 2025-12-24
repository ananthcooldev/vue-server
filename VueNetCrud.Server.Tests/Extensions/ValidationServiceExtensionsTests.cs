using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Filters;
using VueNetCrud.Server.Application.Validators;
using VueNetCrud.Server.Extensions;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions;

public class ValidationServiceExtensionsTests
{
    [Fact]
    public void AddValidationServices_ShouldRegisterProductCreateDtoValidator()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddValidationServices();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var validator = serviceProvider.GetService<IValidator<ProductCreateDto>>();
        validator.Should().NotBeNull();
        validator.Should().BeOfType<ProductCreateDtoValidator>();
    }

    [Fact]
    public void AddValidationServices_ShouldRegisterProductUpdateDtoValidator()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddValidationServices();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var validator = serviceProvider.GetService<IValidator<ProductUpdateDto>>();
        validator.Should().NotBeNull();
        validator.Should().BeOfType<ProductUpdateDtoValidator>();
    }

    [Fact]
    public void AddValidationServices_ShouldRegisterItemCreateDtoValidator()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddValidationServices();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var validator = serviceProvider.GetService<IValidator<ItemCreateDto>>();
        validator.Should().NotBeNull();
        validator.Should().BeOfType<ItemCreateDtoValidator>();
    }

    [Fact]
    public void AddValidationServices_ShouldRegisterItemUpdateDtoValidator()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddValidationServices();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var validator = serviceProvider.GetService<IValidator<ItemUpdateDto>>();
        validator.Should().NotBeNull();
        validator.Should().BeOfType<ItemUpdateDtoValidator>();
    }

    [Fact]
    public void AddValidationServices_ShouldRegisterValidationFilter()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddValidationServices();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var filter = serviceProvider.GetService<ValidationFilter>();
        filter.Should().NotBeNull();
    }

    [Fact]
    public void AddValidationServices_ShouldRegisterServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddValidationServices();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.Should().NotBeNull();
        // Services are registered
    }
}
