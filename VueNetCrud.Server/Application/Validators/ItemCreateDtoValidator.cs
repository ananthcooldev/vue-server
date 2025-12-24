using FluentValidation;
using VueNetCrud.Server.Application.DTOs;

namespace VueNetCrud.Server.Application.Validators;

public class ItemCreateDtoValidator : AbstractValidator<ItemCreateDto>
{
    public ItemCreateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters");
    }
}

