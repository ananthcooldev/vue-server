using FluentValidation;
using VueNetCrud.Server.Models;

namespace VueNetCrud.Server.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name)
              .NotEmpty()
              .Length(3, 100);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.Category)
                .NotEmpty();
        }
    }
}
