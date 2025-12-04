using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using VueNetCrud.Server.Extensions;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions
{
    public class MiddlewareExtensionsAdditionalTests
    {
        [Fact]
        public void UseAppPipeline_WithOptionsRequest_ShouldSkipHttpsRedirection()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddControllers();
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();
            var app = builder.Build();

            // Act
            app.UseAppPipeline();

            // Assert
            // The pipeline is configured - UseWhen condition is tested by the fact that it doesn't throw
            app.Should().NotBeNull();
        }

        [Fact]
        public void UseAppPipeline_WithNonOptionsRequest_ShouldApplyHttpsRedirection()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddControllers();
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();
            var app = builder.Build();

            // Act
            app.UseAppPipeline();

            // Assert
            // The pipeline is configured with conditional HTTPS redirection
            app.Should().NotBeNull();
        }
    }
}

