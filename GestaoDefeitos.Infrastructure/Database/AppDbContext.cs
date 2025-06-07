using GestaoDefeitos.Domain.Entities.Contributor;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestaoDefeitos.Infrastructure.Database
{
    public class AppDbContext : IdentityDbContext<Contributor>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Contributor>()
                .Property(c => c.FullName)
                .HasMaxLength(100);
        }
    }
}
