using VueNetCrud.Server.Repository;

namespace VueNetCrud.Server.Extensions
{
    public static class ServiceExtensions
    {
        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddSingleton<VueNetCrud.Server.Services.ItemRepository>();
            builder.Services.AddSingleton<IProductRepository, ProductRepository>();
        }
    }
}
