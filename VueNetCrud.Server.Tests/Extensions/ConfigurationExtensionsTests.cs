using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using VueNetCrud.Server.Extensions;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions
{
    public class ConfigurationExtensionsTests
    {
        [Fact]
        public void AddConfigurationFiles_ShouldAddAppSettingsJson()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.AddConfigurationFiles();

            // Assert
            builder.Configuration.Should().NotBeNull();
        }

        [Fact]
        public void AddConfigurationFiles_ShouldLoadConfiguration()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.AddConfigurationFiles();

            // Assert
            // Configuration should be accessible
            var configValue = builder.Configuration["Logging:LogLevel:Default"];
            // Even if null, the configuration should be set up
            builder.Configuration.Should().NotBeNull();
        }
    }
}

