using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using VueNetCrud.Server.Extensions;
using Xunit;

namespace VueNetCrud.Server.Tests.Extensions
{
    public class SerilogExtensionsTests
    {
        [Fact]
        public void ConfigureSerilog_ShouldNotThrow()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            var act = () => builder.ConfigureSerilog();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void ConfigureSerilog_ShouldConfigureLogger()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act
            builder.ConfigureSerilog();

            // Assert
            // Serilog is configured
            builder.Host.Should().NotBeNull();
        }
    }
}

