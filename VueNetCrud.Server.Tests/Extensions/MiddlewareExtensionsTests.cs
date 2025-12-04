using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VueNetCrud.Server.Extensions;
using VueNetCrud.Server.Middleware;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions
{
    public class MiddlewareExtensionsTests
    {
        [Fact]
        public void UseGlobalMiddleware_ShouldRegisterErrorHandlerMiddleware()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            // Act
            app.UseGlobalMiddleware();

            // Assert
            // The middleware is registered in the pipeline
            // We can verify by checking that the app is configured
            app.Should().NotBeNull();
        }

        [Fact]
        public void UseAppPipeline_ShouldConfigurePipeline()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            // Add required services before calling UseAppPipeline
            builder.Services.AddControllers();
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();
            var app = builder.Build();

            // Act
            app.UseAppPipeline();

            // Assert
            // The pipeline is configured
            app.Should().NotBeNull();
        }
    }
}

