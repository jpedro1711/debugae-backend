using GestaoDefeitos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDefeitos.Infrastructure.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.Property(p => p.Name)
            .IsRequired()
            .IsUnicode(true);

        builder.Property(p => p.Description)
            .IsRequired()
            .IsUnicode(true);
    }
}
