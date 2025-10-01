using GestaoDefeitos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GestaoDefeitos.Infrastructure.Configurations
{
    public class DefectConfiguration : IEntityTypeConfiguration<Defect>
    {
        public void Configure(EntityTypeBuilder<Defect> builder)
        {
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
