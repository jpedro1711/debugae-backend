using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Infrastructure.Database;
using GestaoDefeitos.WebApi.Endpoints;
using GestaoDefeitos.WebApi.Extensions.Migrations;
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

            return webApplication;
        }
    }
}
