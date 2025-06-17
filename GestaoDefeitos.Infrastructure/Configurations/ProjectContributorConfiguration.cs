using GestaoDefeitos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDefeitos.Infrastructure.Configurations
{
    public class ProjectContributorConfiguration : IEntityTypeConfiguration<ProjectContributor>
    {
        public void Configure(EntityTypeBuilder<ProjectContributor> builder)
        {
            builder.HasKey(pc => new { pc.ProjectId, pc.ContributorId });

            builder.HasOne(pc => pc.Project)
                .WithMany(p => p.ProjectContributors)
                .HasForeignKey(pc => pc.ProjectId);

            builder.HasOne(pc => pc.Contributor)
                .WithMany(c => c.ProjectContributors)
                .HasForeignKey(pc => pc.ContributorId);
        }
    }
}

