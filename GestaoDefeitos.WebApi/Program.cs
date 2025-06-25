using GestaoDefeitos.WebApi.Extensions.ApplicationBuilder;
using QuestPDF.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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
