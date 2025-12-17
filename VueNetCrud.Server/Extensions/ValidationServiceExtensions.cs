using FluentValidation;
using FluentValidation.AspNetCore;
using VueNetCrud.Server.Application.Filters;
using VueNetCrud.Server.Application.Validators;

namespace VueNetCrud.Server.Extensions
{
    public static class ValidationServiceExtensions
    {
        public static void AddValidationServices(this IServiceCollection services)
        {
            // Register FluentValidation validators
            services.AddValidatorsFromAssemblyContaining<ProductCreateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<ProductUpdateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<ItemCreateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<ItemUpdateDtoValidator>();

            // Add validation filter
            services.AddScoped<ValidationFilter>();

            // Configure FluentValidation
            services.AddFluentValidationAutoValidation();
        }
    }
}
