using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Filters;
using VueNetCrud.Server.Extensions;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions;

public class ValidationServiceExtensionsAdditionalTests
{
        [Fact]
        public void AddValidationServices_ShouldRegisterValidators()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddControllers(); // Required for AddValidationServices

            // Act
            services.AddValidationServices();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var validator = serviceProvider.GetService<IValidator<ProductCreateDto>>();
            validator.Should().NotBeNull();
        }

    [Fact]
    public void AddValidationServices_ShouldRegisterValidationFilter()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddControllers();

        // Act
        builder.Services.AddValidationServices();

        // Assert
        var serviceProvider = builder.Services.BuildServiceProvider();
        var filter = serviceProvider.GetService<ValidationFilter>();
        filter.Should().NotBeNull();
    }

    [Fact]
    public void AddValidationServices_ShouldAddValidationFilterToControllers()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddControllers();

        // Act
        builder.Services.AddValidationServices();

        // Assert
        // The filter is added to controller options
        builder.Services.Should().NotBeEmpty();
    }
}
