using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using VueNetCrud.Server.Validators;
using Xunit;

namespace VueNetCrud.Server.Tests.Validators
{
    public class ValidCategoryAttributeTests
    {
        private readonly ValidCategoryAttribute _attribute;

        public ValidCategoryAttributeTests()
        {
            _attribute = new ValidCategoryAttribute();
        }

        [Theory]
        [InlineData("Electronics")]
        [InlineData("Books")]
        [InlineData("Clothing")]
        [InlineData("Sports")]
        public void IsValid_WithValidCategory_ShouldReturnSuccess(string category)
        {
            // Arrange
            var context = new ValidationContext(category);

            // Act
            var result = _attribute.GetValidationResult(category, context);

            // Assert
            result.Should().Be(ValidationResult.Success);
        }

        [Theory]
        [InlineData("Invalid")]
        [InlineData("")]
        [InlineData("electronics")] // case sensitive
        [InlineData("ELECTRONICS")] // case sensitive
        [InlineData("Food")]
        public void IsValid_WithInvalidCategory_ShouldReturnError(string category)
        {
            // Arrange
            var context = new ValidationContext(category);

            // Act
            var result = _attribute.GetValidationResult(category, context);

            // Assert
            result.Should().NotBe(ValidationResult.Success);
            result!.ErrorMessage.Should().Contain("Category must be one of:");
        }

        [Fact]
        public void IsValid_WithNull_ShouldReturnError()
        {
            // Arrange
            // ValidationContext requires a non-null instance, so we use a dummy object
            var dummyObject = new object();
            var context = new ValidationContext(dummyObject);

            // Act
            var result = _attribute.GetValidationResult(null, context);

            // Assert
            result.Should().NotBe(ValidationResult.Success);
            result!.ErrorMessage.Should().Contain("Category must be one of:");
        }
    }
}

