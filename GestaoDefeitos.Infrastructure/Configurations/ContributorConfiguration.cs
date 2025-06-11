using GestaoDefeitos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDefeitos.Infrastructure.Database.Configurations
{
    public class ContributorConfiguration : IEntityTypeConfiguration<Contributor>
    {
        public void Configure(EntityTypeBuilder<Contributor> builder)
        {
            builder.Property(c => c.FullName)
                .HasMaxLength(100);

            builder.Property(c => c.ContributorProfession)
                .IsRequired();
        }
    }
}

