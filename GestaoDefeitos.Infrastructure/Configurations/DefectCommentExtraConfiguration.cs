using GestaoDefeitos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDefeitos.Infrastructure.Configurations;

public class DefectCommentExtraConfiguration : IEntityTypeConfiguration<DefectComment>
{
    public void Configure(EntityTypeBuilder<DefectComment> builder)
    {
        builder.Property(c => c.Content).IsRequired().IsUnicode(true);
    }
}
