using VueNetCrud.Server.Application.Interfaces;
using VueNetCrud.Server.Application.Services;
using VueNetCrud.Server.Domain.Interfaces.Repositories;
using VueNetCrud.Server.Domain.Interfaces.Services;
using VueNetCrud.Server.Infrastructure.Repositories;
using VueNetCrud.Server.Infrastructure.Services;

namespace VueNetCrud.Server.Extensions
{
    public static class ServiceExtensions
    {
        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            // Controllers
            builder.Services.AddControllers();

            // Domain Layer - Repositories
            builder.Services.AddSingleton<IItemRepository, ItemRepository>();
            builder.Services.AddSingleton<IProductRepository, ProductRepository>();

            // Domain Layer - Services
            builder.Services.AddSingleton<ITokenService, TokenService>();

            // Application Layer - Services
            builder.Services.AddScoped<IItemService, ItemService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
        }
    }
}
