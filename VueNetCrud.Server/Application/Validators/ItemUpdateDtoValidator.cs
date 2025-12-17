using FluentValidation;
using VueNetCrud.Server.Application.DTOs;

namespace VueNetCrud.Server.Application.Validators;

public class ItemUpdateDtoValidator : AbstractValidator<ItemUpdateDto>
{
    public ItemUpdateDtoValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));
    }
}

