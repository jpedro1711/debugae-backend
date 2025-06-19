using GestaoDefeitos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDefeitos.Infrastructure.Configurations
{
    public class DefectCommentConfiguration : IEntityTypeConfiguration<DefectComment>
    {
        public void Configure(EntityTypeBuilder<DefectComment> builder)
        {
            builder
                .HasOne(dr => dr.Defect)
                .WithMany(d => d.Comments)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
