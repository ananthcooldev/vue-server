using FluentValidation;
namespace VueNetCrud.Server.Extensions
{
    public static class ValidationServiceExtensions
    {
        public static IServiceCollection AddValidationServices(this IServiceCollection services)
        {

            services.AddValidatorsFromAssemblyContaining<Program>();
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });
            services.AddScoped<ValidationFilter>();

            return services;
        }
    }
}
