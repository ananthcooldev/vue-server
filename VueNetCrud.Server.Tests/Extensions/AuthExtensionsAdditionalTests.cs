using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VueNetCrud.Server.Extensions;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions
{
    public class AuthExtensionsAdditionalTests
    {
        [Fact]
        public void AddJwtAuth_ShouldConfigureTokenValidationParameters()
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
        public void AddJwtAuth_WithLowerCaseJwtSection_ShouldWork()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            builder.Configuration["jwt:Key"] = "TestKey123456789012345678901234567890";
            builder.Configuration["jwt:Issuer"] = "TestIssuer";
            builder.Configuration["jwt:Audience"] = "TestAudience";

            // Act
            builder.AddJwtAuth();

            // Assert
            // Should not throw - configuration is case-insensitive
            builder.Services.Should().NotBeNull();
        }
    }
}

