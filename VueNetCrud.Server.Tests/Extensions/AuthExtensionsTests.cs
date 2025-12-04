using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VueNetCrud.Server.Extensions;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions
{
    public class AuthExtensionsTests
    {
        [Fact]
        public void AddJwtAuth_ShouldRegisterAuthentication()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            builder.Configuration["Jwt:Key"] = "TestKey123456789012345678901234567890";
            builder.Configuration["Jwt:Issuer"] = "TestIssuer";
            builder.Configuration["Jwt:Audience"] = "TestAudience";

            // Act
            builder.AddJwtAuth();

            // Assert
            var serviceProvider = builder.Services.BuildServiceProvider();
            var authSchemeProvider = serviceProvider.GetRequiredService<IAuthenticationSchemeProvider>();
            var schemes = authSchemeProvider.GetAllSchemesAsync().Result;
            schemes.Should().Contain(s => s.Name == JwtBearerDefaults.AuthenticationScheme);
        }

        [Fact]
        public void AddJwtAuth_ShouldRegisterAuthorization()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            builder.Configuration["Jwt:Key"] = "TestKey123456789012345678901234567890";
            builder.Configuration["Jwt:Issuer"] = "TestIssuer";
            builder.Configuration["Jwt:Audience"] = "TestAudience";

            // Act
            builder.AddJwtAuth();

            // Assert
            var serviceProvider = builder.Services.BuildServiceProvider();
            var authService = serviceProvider.GetService<Microsoft.AspNetCore.Authorization.IAuthorizationService>();
            authService.Should().NotBeNull();
        }
    }
}

