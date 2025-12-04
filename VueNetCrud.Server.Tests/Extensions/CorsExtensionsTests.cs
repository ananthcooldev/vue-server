using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using VueNetCrud.Server.Extensions;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions
{
    public class CorsExtensionsTests
    {
        [Fact]
        public void AddCorsPolicy_ShouldRegisterCorsPolicy()
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
        public void AddCorsPolicy_ShouldRegisterClientCorsPolicy()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            builder.AddCorsPolicy();
            var app = builder.Build();

            // Act & Assert
            var serviceProvider = app.Services;
            var corsPolicyProvider = serviceProvider.GetRequiredService<ICorsPolicyProvider>();
            
            // Create a valid HttpContext for the test
            var httpContext = new DefaultHttpContext();
            var policy = corsPolicyProvider.GetPolicyAsync(httpContext, "ClientCors").Result;
            policy.Should().NotBeNull();
        }
    }
}

