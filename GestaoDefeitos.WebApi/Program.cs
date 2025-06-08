using GestaoDefeitos.WebApi.Extensions.ApplicationBuilder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder
    .AddBaseServices()
    .ConfigureIdentity()
    .ConfigureDatabase();

var app = builder.Build();

app
    .ConfigureWebApplication()
    .MapEndpoints();

app.Run();
