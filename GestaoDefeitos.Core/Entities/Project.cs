using GestaoDefeitos.Domain.Entities.RelationEntities;

namespace GestaoDefeitos.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public List<ProjectContributor> ProjectContributors { get; set; } = [];
        public List<Defect> Defects { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
