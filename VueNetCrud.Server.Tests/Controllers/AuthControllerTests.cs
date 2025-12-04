using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using VueNetCrud.Server.Controllers;
using Xunit;

namespace VueNetCrud.Server.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<IConfigurationSection> _mockJwtSection;
        private readonly AuthController _controller;

        public AuthControllerTests()
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
        public void Options_ShouldReturnOk()
        {
            // Act
            var result = _controller.Options();

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void Login_WithValidCredentials_ShouldReturnOkWithToken()
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
            
            // Use reflection to access the token property from anonymous object
            var valueType = okResult.Value!.GetType();
            var tokenProperty = valueType.GetProperty("token");
            Assert.NotNull(tokenProperty);
            var tokenValue = tokenProperty.GetValue(okResult.Value);
            Assert.NotNull(tokenValue);
            Assert.NotEmpty(tokenValue.ToString()!);
        }

        [Fact]
        public void Login_WithInvalidUsername_ShouldReturnUnauthorized()
        {
            // Arrange
            var loginModel = new LoginModel
            {
                Username = "invalid",
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
        public void Login_WithInvalidPassword_ShouldReturnUnauthorized()
        {
            // Arrange
            var loginModel = new LoginModel
            {
                Username = "admin",
                Password = "wrongpassword"
            };

            // Act
            var result = _controller.Login(loginModel);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
            var unauthorizedResult = result as UnauthorizedObjectResult;
            unauthorizedResult!.Value.Should().Be("Invalid credentials");
        }

        [Fact]
        public void Login_WithNullModel_ShouldThrowNullReferenceException()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _controller.Login(null!));
        }
    }
}

