namespace VueNetCrud.Server.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void AddConfigurationFiles(this WebApplicationBuilder builder)
        {
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Logging.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Serilog.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Jwt.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);
        }
    }
}
