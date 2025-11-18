using System.ComponentModel.DataAnnotations;

namespace VueNetCrud.Server.Validators
{
    public class ValidCategoryAttribute : ValidationAttribute
    {
        private readonly string[] _allowedCategories =
            { "Electronics", "Books", "Clothing", "Sports" };

        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            if (value == null || !_allowedCategories.Contains(value.ToString()))
            {
                return new ValidationResult($"Category must be one of: {string.Join(", ", _allowedCategories)}");
            }

            return ValidationResult.Success;
        }
    }
}
