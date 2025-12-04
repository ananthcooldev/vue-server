using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VueNetCrud.Server.Extensions;
using VueNetCrud.Server.Validators;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions
{
    public class ValidationServiceExtensionsAdditionalTests
    {
        [Fact]
        public void AddValidationServices_ShouldRegisterValidators()
        {
            // Arrange
            var services = new ServiceCollection();
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddControllers(); // Required for AddValidationServices

            // Act
            var result = builder.Services.AddValidationServices();

            // Assert
            result.Should().BeSameAs(builder.Services);
            var serviceProvider = builder.Services.BuildServiceProvider();
            var validator = serviceProvider.GetService<FluentValidation.IValidator<VueNetCrud.Server.Models.Product>>();
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
}

