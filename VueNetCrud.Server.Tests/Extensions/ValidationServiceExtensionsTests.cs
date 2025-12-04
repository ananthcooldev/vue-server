using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VueNetCrud.Server.Extensions;
using VueNetCrud.Server.Models;
using VueNetCrud.Server.Validators;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions
{
    public class ValidationServiceExtensionsTests
    {
        [Fact]
        public void AddValidationServices_ShouldRegisterProductValidator()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddValidationServices();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var validator = serviceProvider.GetService<IValidator<Product>>();
            validator.Should().NotBeNull();
            validator.Should().BeOfType<ProductValidator>();
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
        public void AddValidationServices_ShouldReturnServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddValidationServices();

            // Assert
            result.Should().BeSameAs(services);
        }
    }
}

