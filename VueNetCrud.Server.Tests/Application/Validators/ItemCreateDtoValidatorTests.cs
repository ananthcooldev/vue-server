using FluentAssertions;
using VueNetCrud.Server.Application.DTOs;
using VueNetCrud.Server.Application.Validators;
using Xunit;

namespace VueNetCrud.Server.Tests.Application.Validators;

public class ItemCreateDtoValidatorTests
{
    private readonly ItemCreateDtoValidator _validator;

    public ItemCreateDtoValidatorTests()
    {
        _validator = new ItemCreateDtoValidator();
    }

    [Fact]
    public void Validate_WithValidDto_ShouldPass()
    {
        // Arrange
        var dto = new ItemCreateDto("Valid Item Name", "Description");

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithEmptyName_ShouldFail()
    {
        // Arrange
        var dto = new ItemCreateDto("", "Description");

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name" && e.ErrorMessage == "Name is required");
    }

    [Fact]
    public void Validate_WithNullName_ShouldFail()
    {
        // Arrange
        var dto = new ItemCreateDto(null!, "Description");

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validate_WithNameExceedingMaxLength_ShouldFail()
    {
        // Arrange
        var longName = new string('A', 201);
        var dto = new ItemCreateDto(longName, "Description");

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name" && e.ErrorMessage.Contains("200 characters"));
    }

    [Fact]
    public void Validate_WithValidNameAndNullDescription_ShouldPass()
    {
        // Arrange
        var dto = new ItemCreateDto("Valid Name", null);

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}

