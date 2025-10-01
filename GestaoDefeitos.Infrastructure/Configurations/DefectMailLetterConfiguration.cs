using GestaoDefeitos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDefeitos.Infrastructure.Configurations
{
    public class DefectMailLetterConfiguration : IEntityTypeConfiguration<DefectMailLetter>
    {
        public void Configure(EntityTypeBuilder<DefectMailLetter> builder)
        {
            builder
                .HasKey(dr => new { dr.DefectId, dr.ContributorId });

            builder
                .HasOne(dr => dr.Defect)
                .WithMany(d => d.ContributorMailLetter)
                .HasForeignKey(dr => dr.DefectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(dr => dr.Contributor)
                .WithMany(d => d.DefectsSubscribedAtMailLetter)
                .HasForeignKey(dr => dr.ContributorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
