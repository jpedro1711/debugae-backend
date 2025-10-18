using GestaoDefeitos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDefeitos.Infrastructure.Configurations
{
    public class DefectConfiguration : IEntityTypeConfiguration<Defect>
    {
        public void Configure(EntityTypeBuilder<Defect> builder)
        {
            // String properties as NVARCHAR (Unicode)
            builder.Property(d => d.Summary)
                .IsRequired()
                .IsUnicode(true);

            builder.Property(d => d.Description)
                .IsRequired()
                .IsUnicode(true);

            builder.Property(d => d.Version)
                .IsRequired()
                .IsUnicode(true);

            builder.Property(d => d.ExpectedBehaviour)
                .IsRequired()
                .IsUnicode(true);

            builder.Property(d => d.ActualBehaviour)
                .IsRequired()
                .IsUnicode(true);

            builder.Property(d => d.ErrorLog)
                .IsUnicode(true);

            // Relationships
            builder.HasOne(d => d.AssignedToContributor)
                .WithMany(c => c.DefectsAssignedTo)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(d => d.Comments)
                .WithOne(c => c.Defect);

            builder.HasMany(d => d.TrelloUserStories)
                .WithOne(t => t.Defect);

            builder.HasMany(d => d.DefectHistory)
                .WithOne(e => e.Defect);
        }
    }
}
