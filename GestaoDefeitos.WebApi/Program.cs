using GestaoDefeitos.WebApi.Extensions.ApplicationBuilder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder
    .AddBaseServices()
    .ConfigureDI()
    .ConfigureMediator()
    .ConfigureIdentity()
    .ConfigureDatabase();

var app = builder.Build();

app
    .UseMiddlewares()
    .ConfigureWebApplication()
    .MapEndpoints();

await app.RunAsync();
