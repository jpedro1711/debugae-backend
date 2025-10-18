using GestaoDefeitos.Infrastructure.Database;
using GestaoDefeitos.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

namespace GestaoDefeitos.WebApi.Extensions.Migrations
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();

            // Run seed (fire-and-forget acceptable at startup, but await synchronously to ensure data exists)
            DatabaseSeeder.SeedAsync(app.Services).GetAwaiter().GetResult();
        }
    }
}
