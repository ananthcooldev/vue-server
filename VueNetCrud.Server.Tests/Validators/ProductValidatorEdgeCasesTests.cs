using FluentAssertions;
using FluentValidation.TestHelper;
using VueNetCrud.Server.Models;
using VueNetCrud.Server.Validators;
using Xunit;

namespace VueNetCrud.Server.Tests.Validators
{
    public class ProductValidatorEdgeCasesTests
    {
        private readonly ProductValidator _validator;

        public ProductValidatorEdgeCasesTests()
        {
            _validator = new ProductValidator();
        }

        [Fact]
        public void Validate_WithVeryLongName_ShouldFail()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = new string('A', 101),
                Price = 100,
                Category = "Electronics"
            };

            // Act
            var result = _validator.TestValidate(product);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_WithExactMaxLengthName_ShouldPass()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = new string('A', 100),
                Price = 100,
                Category = "Electronics"
            };

            // Act
            var result = _validator.TestValidate(product);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_WithExactMinLengthName_ShouldPass()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = new string('A', 3),
                Price = 100,
                Category = "Electronics"
            };

            // Act
            var result = _validator.TestValidate(product);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_WithVeryLargePrice_ShouldPass()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Valid Product",
                Price = decimal.MaxValue,
                Category = "Electronics"
            };

            // Act
            var result = _validator.TestValidate(product);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void Validate_WithVerySmallPositivePrice_ShouldPass()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Valid Product",
                Price = 0.01m,
                Category = "Electronics"
            };

            // Act
            var result = _validator.TestValidate(product);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void Validate_WithWhitespaceName_ShouldFail()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "   ",
                Price = 100,
                Category = "Electronics"
            };

            // Act
            var result = _validator.TestValidate(product);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_WithWhitespaceCategory_ShouldFail()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Valid Product",
                Price = 100,
                Category = "   "
            };

            // Act
            var result = _validator.TestValidate(product);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Category);
        }
    }
}

