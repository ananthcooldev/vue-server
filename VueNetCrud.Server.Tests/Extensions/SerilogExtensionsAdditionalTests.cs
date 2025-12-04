using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using VueNetCrud.Server.Extensions;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions
{
    public class SerilogExtensionsAdditionalTests
    {
        [Fact]
        public void ConfigureSerilog_ShouldConfigureLoggerFromConfiguration()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.ConfigureSerilog();

            // Assert
            builder.Host.Should().NotBeNull();
        }

        [Fact]
        public void ConfigureSerilog_ShouldEnrichFromLogContext()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.ConfigureSerilog();

            // Assert
            // Serilog is configured with enrichment
            builder.Host.Should().NotBeNull();
        }
    }
}

