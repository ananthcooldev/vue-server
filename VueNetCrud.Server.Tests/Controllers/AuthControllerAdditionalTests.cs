using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using VueNetCrud.Server.Controllers;
using Xunit;

namespace VueNetCrud.Server.Tests.Controllers
{
    public class AuthControllerAdditionalTests
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<IConfigurationSection> _mockJwtSection;
        private readonly AuthController _controller;

        public AuthControllerAdditionalTests()
        {
            _mockConfig = new Mock<IConfiguration>();
            _mockJwtSection = new Mock<IConfigurationSection>();

            _mockJwtSection.Setup(x => x["Key"]).Returns("ThisIsASecretKeyForJwtTokenGeneration123456");
            _mockJwtSection.Setup(x => x["Issuer"]).Returns("TestIssuer");
            _mockJwtSection.Setup(x => x["Audience"]).Returns("TestAudience");

            _mockConfig.Setup(x => x.GetSection("Jwt")).Returns(_mockJwtSection.Object);

            _controller = new AuthController(_mockConfig.Object);
        }

        [Fact]
        public void Options_WithLoginRoute_ShouldReturnOk()
        {
            // Act
            var result = _controller.Options();

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void Login_WithEmptyUsername_ShouldReturnUnauthorized()
        {
            // Arrange
            var loginModel = new LoginModel
            {
                Username = "",
                Password = "123"
            };

            // Act
            var result = _controller.Login(loginModel);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
            var unauthorizedResult = result as UnauthorizedObjectResult;
            unauthorizedResult!.Value.Should().Be("Invalid credentials");
        }

        [Fact]
        public void Login_WithEmptyPassword_ShouldReturnUnauthorized()
        {
            // Arrange
            var loginModel = new LoginModel
            {
                Username = "admin",
                Password = ""
            };

            // Act
            var result = _controller.Login(loginModel);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
            var unauthorizedResult = result as UnauthorizedObjectResult;
            unauthorizedResult!.Value.Should().Be("Invalid credentials");
        }

        [Fact]
        public void Login_WithWhitespaceCredentials_ShouldReturnUnauthorized()
        {
            // Arrange
            var loginModel = new LoginModel
            {
                Username = "   ",
                Password = "   "
            };

            // Act
            var result = _controller.Login(loginModel);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public void Login_ShouldGenerateValidJwtToken()
        {
            // Arrange
            var loginModel = new LoginModel
            {
                Username = "admin",
                Password = "123"
            };

            // Act
            var result = _controller.Login(loginModel);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().NotBeNull();
            
            var valueType = okResult.Value!.GetType();
            var tokenProperty = valueType.GetProperty("token");
            Assert.NotNull(tokenProperty);
            var tokenValue = tokenProperty.GetValue(okResult.Value);
            Assert.NotNull(tokenValue);
            var tokenString = tokenValue.ToString()!;
            Assert.NotEmpty(tokenString);
            
            // Verify it's a valid JWT format (has 3 parts separated by dots)
            var parts = tokenString.Split('.');
            parts.Length.Should().Be(3);
        }
    }
}

