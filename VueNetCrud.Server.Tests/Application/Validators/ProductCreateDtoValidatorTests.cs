using FluentAssertions;
using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Validators;
using Xunit;

namespace VueNetCrud.Server.Tests.Application.Validators;

public class ProductCreateDtoValidatorTests
{
    private readonly ProductCreateDtoValidator _validator;

    public ProductCreateDtoValidatorTests()
    {
        _validator = new ProductCreateDtoValidator();
    }

    [Fact]
    public void Validate_WithValidDto_ShouldPass()
    {
        // Arrange
        var dto = new ProductCreateDto("Valid Product", 100, "Electronics");

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithEmptyName_ShouldFail()
    {
        // Arrange
        var dto = new ProductCreateDto("", 100, "Electronics");

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validate_WithNameTooShort_ShouldFail()
    {
        // Arrange
        var dto = new ProductCreateDto("AB", 100, "Electronics");

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validate_WithNameTooLong_ShouldFail()
    {
        // Arrange
        var longName = new string('A', 101);
        var dto = new ProductCreateDto(longName, 100, "Electronics");

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validate_WithZeroPrice_ShouldFail()
    {
        // Arrange
        var dto = new ProductCreateDto("Valid Product", 0, "Electronics");

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Price");
    }

    [Fact]
    public void Validate_WithNegativePrice_ShouldFail()
    {
        // Arrange
        var dto = new ProductCreateDto("Valid Product", -10, "Electronics");

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Price");
    }

    [Fact]
    public void Validate_WithEmptyCategory_ShouldFail()
    {
        // Arrange
        var dto = new ProductCreateDto("Valid Product", 100, "");

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Category");
    }

    [Fact]
    public void Validate_WithInvalidCategory_ShouldFail()
    {
        // Arrange
        var dto = new ProductCreateDto("Valid Product", 100, "InvalidCategory");

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Category" && e.ErrorMessage.Contains("Electronics, Books, Clothing, Sports"));
    }

    [Theory]
    [InlineData("Electronics")]
    [InlineData("Books")]
    [InlineData("Clothing")]
    [InlineData("Sports")]
    public void Validate_WithValidCategory_ShouldPass(string category)
    {
        // Arrange
        var dto = new ProductCreateDto("Valid Product", 100, category);

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}

