using VueNetCrud.Server.Middleware;

namespace VueNetCrud.Server.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void UseGlobalMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }

        public static void UseAppPipeline(this WebApplication app)
        {
            // Skip HTTPS redirection for OPTIONS requests to allow CORS preflight
            app.UseWhen(context => context.Request.Method != "OPTIONS", appBuilder =>
            {
                appBuilder.UseHttpsRedirection();
            });
            
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
