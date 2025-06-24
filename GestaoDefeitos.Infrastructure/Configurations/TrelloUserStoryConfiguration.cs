using GestaoDefeitos.Application.TrelloIntegration;
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
        }
    }
}
