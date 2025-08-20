using GestaoDefeitos.Application.Assembly;
using GestaoDefeitos.Application.Cache;
using GestaoDefeitos.Application.TrelloIntegration;
using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;
using GestaoDefeitos.Infrastructure.Repositories;
using GestaoDefeitos.WebApi.Endpoints;
using GestaoDefeitos.WebApi.Extensions.Migrations;
using GestaoDefeitos.WebApi.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace GestaoDefeitos.WebApi.Extensions.ApplicationBuilder
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddBaseServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<AuthenticationContextAcessor>();
            builder.Services.AddAuthorization();

            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.WithOrigins(allowedOrigins!)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

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
            builder.Services.AddScoped<IDefectRelationRepository, DefectRelationRepository>();
            builder.Services.AddScoped<ITrelloUserStoryRepository, TrelloUserStoryRepository>();

            builder.Services.AddScoped<ITrelloIntegrationService, TrelloIntegrationService>();
            builder.Services.AddScoped<ITrelloRequestTokenCache, TrelloRequestTokenCach>();

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
                options.Cookie.Name = "auth_cookie";
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.SlidingExpiration = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.HttpOnly = false;
                options.LoginPath = string.Empty;
                options.AccessDeniedPath = string.Empty;

                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            });

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Regras de senha
                options.Password.RequireDigit = false;             // Precisa de número
                options.Password.RequiredLength = 2;              // Tamanho mínimo
                options.Password.RequireNonAlphanumeric = false;  // Não exige caractere especial
                options.Password.RequireUppercase = false;         // Precisa de letra maiúscula
                options.Password.RequireLowercase = false;         // Precisa de letra minúscula
                options.Password.RequiredUniqueChars = 1;         // Caracteres únicos mínimos

                // Outras regras
                options.User.RequireUniqueEmail = true;           // Email único por usuário
            });


            builder.Services.AddMemoryCache();
            builder.Services.Configure<TrelloApiOptions>(builder.Configuration.GetSection("TrelloApiOptions"));
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
            webApplication.UseSwagger();
            webApplication.UseSwaggerUI();

            webApplication.ApplyMigrations();

            webApplication.UseHttpsRedirection();

            webApplication.UseCors("AllowAll");


            webApplication.UseAuthentication();
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
            webApplication.MapNotificationEndpoints();
            webApplication.MapReportEndpoints();

            return webApplication;
        }

        public static WebApplication UseMiddlewares(this WebApplication webApplication)
        {
            webApplication.UseMiddleware<ErrorMiddleware>();

            return webApplication;
        }
    }
}
