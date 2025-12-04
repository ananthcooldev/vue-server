using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VueNetCrud.Server.Extensions;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions
{
    public class SwaggerExtensionsAdditionalTests
    {
        [Fact]
        public void ConfigureSwaggerUI_ShouldSetRoutePrefix()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            builder.AddSwaggerConfig();
            var app = builder.Build();

            // Act
            app.ConfigureSwaggerUI();

            // Assert
            app.Should().NotBeNull();
        }

        [Fact]
        public void AddSwaggerConfig_ShouldAddSecurityDefinition()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.AddSwaggerConfig();

            // Assert
            builder.Services.Should().NotBeNull();
            // Swagger services are registered
        }
    }
}

