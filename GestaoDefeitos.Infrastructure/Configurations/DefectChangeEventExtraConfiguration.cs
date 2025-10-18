using GestaoDefeitos.Domain.Entities.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDefeitos.Infrastructure.Configurations;

public class DefectChangeEventExtraConfiguration : IEntityTypeConfiguration<DefectChangeEvent>
{
    public void Configure(EntityTypeBuilder<DefectChangeEvent> builder)
    {
        builder.Property(e => e.Field).IsUnicode(true);
        builder.Property(e => e.OldValue).IsUnicode(true);
        builder.Property(e => e.NewValue).IsUnicode(true);
    }
}
