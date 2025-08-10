using GestaoDefeitos.Domain.Entities.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoDefeitos.Infrastructure.Configurations
{
    public class DefectChangeEventConfigurations : IEntityTypeConfiguration<DefectChangeEvent>
    {
        public void Configure(EntityTypeBuilder<DefectChangeEvent> builder)
        {
            builder
                .HasOne(dr => dr.Defect)
                .WithMany(d => d.DefectHistory)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
