using VueNetCrud.Server.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.AddConfigurationFiles();
builder.ConfigureSerilog();
builder.RegisterServices();
builder.AddJwtAuth();
builder.AddSwaggerConfig();
builder.AddCorsPolicy();
builder.Services.AddValidationServices();

var app = builder.Build();

// CORS MUST be the very first middleware - before anything else
// This handles OPTIONS preflight requests automatically
app.UseCors("ClientCors");

app.UseGlobalMiddleware();

// Only enable Swagger in Development
if (!app.Environment.IsProduction())
{
    app.ConfigureSwaggerUI();
}

app.UseAppPipeline();
app.Run();
