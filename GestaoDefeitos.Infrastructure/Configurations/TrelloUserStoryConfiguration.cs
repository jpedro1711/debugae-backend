using GestaoDefeitos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDefeitos.Infrastructure.Configurations
{
    public class TrelloUserStoryConfiguration : IEntityTypeConfiguration<TrelloUserStory>
    {
        public void Configure(EntityTypeBuilder<TrelloUserStory> builder)
        {
            builder
                .HasOne(dr => dr.Defect)
                .WithMany(d => d.TrelloUserStories)
                .OnDelete(DeleteBehavior.ClientCascade);

            // Ensure Unicode for text fields
            builder.Property(t => t.Name).IsUnicode(true);
            builder.Property(t => t.Desc).IsUnicode(true);
            builder.Property(t => t.ShortUrl).IsUnicode(true);
        }
    }
}
