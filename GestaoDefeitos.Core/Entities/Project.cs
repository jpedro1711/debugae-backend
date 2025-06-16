using GestaoDefeitos.Domain.Entities.RelationEntities;

namespace GestaoDefeitos.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<ProjectContributor> ProjectContributors { get; set; } = [];
        public List<Defect> Defects { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
