using GestaoDefeitos.WebApi.Extensions.ApplicationBuilder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder
    .AddBaseServices()
    .ConfigureDI()
    .ConfigureIdentity()
    .ConfigureDatabase();

var app = builder.Build();

app
    .ConfigureWebApplication()
    .MapEndpoints();

app.Run();
