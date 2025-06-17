using GestaoDefeitos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDefeitos.Infrastructure.Configurations
{
    public class ContributorConfiguration : IEntityTypeConfiguration<Contributor>
    {
        public void Configure(EntityTypeBuilder<Contributor> builder)
        {
            builder.Property(c => c.Firstname)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.Lastname)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.Department)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.ContributorProfession)
                .IsRequired();
        }
    }
}

