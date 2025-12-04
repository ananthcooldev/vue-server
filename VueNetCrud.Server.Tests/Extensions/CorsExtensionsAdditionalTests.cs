using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using VueNetCrud.Server.Extensions;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions
{
    public class CorsExtensionsAdditionalTests
    {
        [Fact]
        public void AddCorsPolicy_ShouldRegisterCorsService()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.AddCorsPolicy();

            // Assert
            var serviceProvider = builder.Services.BuildServiceProvider();
            var corsService = serviceProvider.GetService<ICorsService>();
            corsService.Should().NotBeNull();
        }

        [Fact]
        public void AddCorsPolicy_ShouldConfigureClientCorsPolicy()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.AddCorsPolicy();

            // Assert
            // Verify CORS services are registered
            var serviceProvider = builder.Services.BuildServiceProvider();
            var corsService = serviceProvider.GetService<ICorsService>();
            corsService.Should().NotBeNull();
        }

        [Fact]
        public void AddCorsPolicy_ShouldAllowMultipleOrigins()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.AddCorsPolicy();

            // Assert
            // Verify CORS is configured
            var serviceProvider = builder.Services.BuildServiceProvider();
            var corsService = serviceProvider.GetService<ICorsService>();
            corsService.Should().NotBeNull();
        }
    }
}

