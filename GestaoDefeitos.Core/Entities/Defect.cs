using GestaoDefeitos.Domain.Entities.Events;
using GestaoDefeitos.Domain.Enums;

namespace GestaoDefeitos.Domain.Entities
{
    public class Defect
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        public Guid AssignedToContributorId { get; set; }
        public Contributor AssignedToContributor { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DefectCategory DefectCategory { get; set; }
        public DefectSeverity DefectSeverity { get; set; }
        public DefectEnvironment DefectEnvironment { get; set; }
        public DefectPriority DefectPriority { get; set; }
        public string Version { get; set; } = null!;
        public string ExpectedBehaviour { get; set; } = null!;
        public string ActualBehaviour { get; set; } = null!;
        public string ErrorLog { get; set; } = string.Empty;
        public DefectAttachment? Attachment { get; set; } = null!;
        public DateTime ExpiresIn { get; set; }
        public DefectStatus Status { get; set; }
        public List<DefectComment> Comments { get; set; } = [];
        public List<DefectRelation> RelatedDefects { get; set; } = [];
        public List<DefectRelation> RelatedToDefects { get; set; } = [];
        public List<DefectChangeEvent> DefectHistory { get; set; } = [];
        public List<TrelloUserStory> TrelloUserStories { get; set; } = [];
        public List<Tag> Tags { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
