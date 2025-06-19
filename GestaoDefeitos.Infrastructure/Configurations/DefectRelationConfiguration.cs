using GestaoDefeitos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace GestaoDefeitos.Infrastructure.Configurations
{
    public class DefectRelationConfiguration : IEntityTypeConfiguration<DefectRelation>
    {
        public void Configure(EntityTypeBuilder<DefectRelation> builder)
        {
            builder
                .HasKey(dr => new { dr.DefectId, dr.RelatedDefectId });

            builder
                .HasOne(dr => dr.Defect)
                .WithMany(d => d.RelatedDefects)
                .HasForeignKey(dr => dr.DefectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(dr => dr.RelatedDefect)
                .WithMany(d => d.RelatedToDefects)
                .HasForeignKey(dr => dr.RelatedDefectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
