using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using VueNetCrud.Server.Validators;
using Xunit;

namespace VueNetCrud.Server.Tests.Validators
{
    public class ValidCategoryAttributeEdgeCasesTests
    {
        private readonly ValidCategoryAttribute _attribute;

        public ValidCategoryAttributeEdgeCasesTests()
        {
            _attribute = new ValidCategoryAttribute();
        }

        [Theory]
        [InlineData("Electronics ")] // trailing space
        [InlineData(" Electronics")] // leading space
        [InlineData(" Electronics ")] // both spaces
        public void IsValid_WithWhitespace_ShouldReturnError(string category)
        {
            // Arrange
            var dummyObject = new object();
            var context = new ValidationContext(dummyObject);

            // Act
            var result = _attribute.GetValidationResult(category, context);

            // Assert
            result.Should().NotBe(ValidationResult.Success);
            result!.ErrorMessage.Should().Contain("Category must be one of:");
        }

        [Fact]
        public void IsValid_WithWhitespaceOnly_ShouldReturnError()
        {
            // Arrange
            var dummyObject = new object();
            var context = new ValidationContext(dummyObject);

            // Act
            var result = _attribute.GetValidationResult("   ", context);

            // Assert
            result.Should().NotBe(ValidationResult.Success);
            result!.ErrorMessage.Should().Contain("Category must be one of:");
        }
    }
}

