using FluentValidation;
using VueNetCrud.Server.Application.DTOs;

namespace VueNetCrud.Server.Application.Validators;

public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
{
    public ProductUpdateDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.Category)
            .NotEmpty()
            .Must(category => new[] { "Electronics", "Books", "Clothing", "Sports" }.Contains(category))
            .WithMessage("Category must be one of: Electronics, Books, Clothing, Sports");
    }
}

