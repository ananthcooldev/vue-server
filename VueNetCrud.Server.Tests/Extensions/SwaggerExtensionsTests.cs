using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VueNetCrud.Server.Extensions;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions
{
    public class SwaggerExtensionsTests
    {
        [Fact]
        public void AddSwaggerConfig_ShouldRegisterSwaggerServices()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.AddSwaggerConfig();

            // Assert
            var serviceProvider = builder.Services.BuildServiceProvider();
            // Swagger services are registered internally
            builder.Services.Should().Contain(s => s.ServiceType.Name.Contains("Swagger"));
        }

        [Fact]
        public void ConfigureSwaggerUI_ShouldNotThrow()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            builder.AddSwaggerConfig();
            var app = builder.Build();

            // Act
            var act = () => app.ConfigureSwaggerUI();

            // Assert
            act.Should().NotThrow();
        }
    }
}

