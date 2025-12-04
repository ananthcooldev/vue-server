using FluentAssertions;
using FluentValidation.TestHelper;
using VueNetCrud.Server.Models;
using VueNetCrud.Server.Validators;
using Xunit;

namespace VueNetCrud.Server.Tests.Validators
{
    public class ProductValidatorTests
    {
        private readonly ProductValidator _validator;

        public ProductValidatorTests()
        {
            _validator = new ProductValidator();
        }

        [Fact]
        public void Validate_WithValidProduct_ShouldPass()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Valid Product",
                Price = 100,
                Category = "Electronics"
            };

            // Act
            var result = _validator.TestValidate(product);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WithEmptyName_ShouldFail()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "",
                Price = 100,
                Category = "Electronics"
            };

            // Act
            var result = _validator.TestValidate(product);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_WithNameTooShort_ShouldFail()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "AB",
                Price = 100,
                Category = "Electronics"
            };

            // Act
            var result = _validator.TestValidate(product);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_WithNameTooLong_ShouldFail()
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
        public void Validate_WithNameAtMinimumLength_ShouldPass()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "ABC",
                Price = 100,
                Category = "Electronics"
            };

            // Act
            var result = _validator.TestValidate(product);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_WithNameAtMaximumLength_ShouldPass()
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
        public void Validate_WithZeroPrice_ShouldFail()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Valid Product",
                Price = 0,
                Category = "Electronics"
            };

            // Act
            var result = _validator.TestValidate(product);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void Validate_WithNegativePrice_ShouldFail()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Valid Product",
                Price = -10,
                Category = "Electronics"
            };

            // Act
            var result = _validator.TestValidate(product);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void Validate_WithPositivePrice_ShouldPass()
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
        public void Validate_WithEmptyCategory_ShouldFail()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Valid Product",
                Price = 100,
                Category = ""
            };

            // Act
            var result = _validator.TestValidate(product);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Category);
        }
    }
}

