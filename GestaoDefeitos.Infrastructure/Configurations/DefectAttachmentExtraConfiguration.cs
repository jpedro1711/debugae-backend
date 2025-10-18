using GestaoDefeitos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDefeitos.Infrastructure.Configurations;

public class DefectAttachmentExtraConfiguration : IEntityTypeConfiguration<DefectAttachment>
{
    public void Configure(EntityTypeBuilder<DefectAttachment> builder)
    {
        builder.Property(a => a.FileName).IsRequired().IsUnicode(true);
        builder.Property(a => a.FileType).IsRequired().IsUnicode(true);
        builder.Property(a => a.UploadByUsername).IsRequired().IsUnicode(true);
    }
}
