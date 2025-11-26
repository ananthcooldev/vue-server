namespace VueNetCrud.Server.Extensions
{
    public static class CorsExtensions
    {
        public static void AddCorsPolicy(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ClientCors", policy =>
                {
                    policy.WithOrigins(
                            "http://localhost:5173",
                            "http://localhost:5174",
                            "http://localhost:3000",
                            "http://localhost:8080",
                            "https://local.vueclient.com",
                            "http://local.vueclient.com",
                            "http://localhost:4200",
                            "https://local.angularclient.com"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
                });
            });
        }
    }
}
