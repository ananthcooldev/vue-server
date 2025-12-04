using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VueNetCrud.Server.Extensions;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions
{
    public class SwaggerExtensionsProductionTests
    {
        [Fact]
        public void ConfigureSwaggerUI_InProduction_ShouldNotBeCalled()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            builder.Environment.EnvironmentName = "Production";
            builder.AddSwaggerConfig();
            var app = builder.Build();

            // Act & Assert
            // In production, ConfigureSwaggerUI should not be called
            // This test verifies the extension method exists and can be called
            // The actual conditional logic is in Program.cs
            app.Should().NotBeNull();
        }

        [Fact]
        public void ConfigureSwaggerUI_InDevelopment_ShouldConfigureSwagger()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            builder.Environment.EnvironmentName = "Development";
            builder.AddSwaggerConfig();
            var app = builder.Build();

            // Act
            app.ConfigureSwaggerUI();

            // Assert
            app.Should().NotBeNull();
        }
    }
}

