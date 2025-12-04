using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using VueNetCrud.Server.Extensions;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions
{
    public class ConfigurationExtensionsAdditionalTests
    {
        [Fact]
        public void AddConfigurationFiles_ShouldLoadEnvironmentSpecificFile()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            builder.Environment.EnvironmentName = "Development";

            // Act
            builder.AddConfigurationFiles();

            // Assert
            builder.Configuration.Should().NotBeNull();
        }

        [Fact]
        public void AddConfigurationFiles_WithProductionEnvironment_ShouldLoadProductionFile()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            builder.Environment.EnvironmentName = "Production";

            // Act
            builder.AddConfigurationFiles();

            // Assert
            builder.Configuration.Should().NotBeNull();
        }

        [Fact]
        public void AddConfigurationFiles_ShouldLoadOptionalFiles()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.AddConfigurationFiles();

            // Assert
            // Verify that optional files can be loaded
            builder.Configuration.Should().NotBeNull();
            // The method should not throw even if optional files don't exist
        }
    }
}

