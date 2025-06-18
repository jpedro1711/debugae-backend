using GestaoDefeitos.Application.Assembly;
using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;
using GestaoDefeitos.Infrastructure.Repositories;
using GestaoDefeitos.WebApi.Endpoints;
using GestaoDefeitos.WebApi.Extensions.Migrations;
using GestaoDefeitos.WebApi.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GestaoDefeitos.WebApi.Extensions.ApplicationBuilder
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddBaseServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthorization();

            return builder;
        }

        public static WebApplicationBuilder ConfigureDI(this WebApplicationBuilder builder)
        {
            builder.ConfigureRepositories();

            return builder;
        }

        public static WebApplicationBuilder ConfigureRepositories(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IDefectRepository, DefectRepository>();
            builder.Services.AddScoped<IContributorRepository, ContributorRepository>();
            builder.Services.AddScoped<IContributorNotificationRepository, ContributorNotificationRepository>();
            builder.Services.AddScoped<IDefectAttachmentRepository, DefectAttachmentRepository>();
            builder.Services.AddScoped<IDefectCommentRepository, DefectCommentRepository>();
            builder.Services.AddScoped<IDefectHistoryRepository, DefectHistoryRepository>();
            builder.Services.AddScoped<IProjectContributorRepository, ProjectContributorRepository>();
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();

            return builder;
        }

        public static WebApplicationBuilder ConfigureMediator(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IApplicationMarker>());
            return builder;
        }

        public static WebApplicationBuilder ConfigureIdentity(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            })
            .AddCookie(IdentityConstants.ApplicationScheme, options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.SlidingExpiration = true;
            })
            .AddBearerToken(IdentityConstants.BearerScheme);

            builder.Services.AddIdentityCore<Contributor>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddApiEndpoints();

            return builder;
        }

        public static WebApplicationBuilder ConfigureDatabase(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            return builder;
        }

        public static WebApplication ConfigureWebApplication(this WebApplication webApplication)
        {
            if (webApplication.Environment.IsDevelopment())
            {
                webApplication.UseSwagger();
                webApplication.UseSwaggerUI();
            }

            webApplication.ApplyMigrations();

            webApplication.UseHttpsRedirection();

            webApplication.UseAuthorization();

            webApplication.MapControllers();

            webApplication.MapIdentityApi<Contributor>();

            return webApplication;
        }

        public static WebApplication MapEndpoints(this WebApplication webApplication)
        {
            webApplication.MapContributorEndpoints();
            webApplication.MapProjectEndpoints();
            webApplication.MapDefectEndpoints();

            return webApplication;
        }

        public static WebApplication UseMiddlewares(this WebApplication webApplication)
        {
            webApplication.UseMiddleware<ErrorMiddleware>();

            return webApplication;
        }
    }
}
