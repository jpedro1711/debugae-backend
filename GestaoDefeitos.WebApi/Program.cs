using GestaoDefeitos.WebApi.Extensions.ApplicationBuilder;
using QuestPDF.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder
    .AddBaseServices()
    .ConfigureDI()
    .ConfigureMediator()
    .ConfigureIdentity()
    .ConfigureDatabase();

var app = builder.Build();

QuestPDF.Settings.License = LicenseType.Community;

app
    .UseMiddlewares()
    .ConfigureWebApplication()
    .MapEndpoints();

await app.RunAsync();
