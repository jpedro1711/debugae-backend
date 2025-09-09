using GestaoDefeitos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDefeitos.Infrastructure.Configurations
{
    public class DefectAttachmentConfiguration : IEntityTypeConfiguration<DefectAttachment>
    {
        public void Configure(EntityTypeBuilder<DefectAttachment> builder)
        {
            builder
                .HasOne(dr => dr.Defect)
                .WithOne(d => d.Attachment)
                .HasForeignKey<DefectAttachment>(dr => dr.DefectId);
        }
    }
}
