using GestaoDefeitos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDefeitos.Infrastructure.Configurations
{
    public class DefectHistoryConfigurations : IEntityTypeConfiguration<DefectHistory>
    {
        public void Configure(EntityTypeBuilder<DefectHistory> builder)
        {
            builder
                .HasOne(dr => dr.Defect)
                .WithMany(d => d.DefectHistory)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
