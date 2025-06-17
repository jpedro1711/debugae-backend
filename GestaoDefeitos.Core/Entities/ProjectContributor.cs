using GestaoDefeitos.Domain.Enums;

namespace GestaoDefeitos.Domain.Entities
{
    public class ProjectContributor
    {
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        public Guid ContributorId { get; set; }
        public Contributor Contributor { get; set; } = null!;
        public ProjectRole Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
