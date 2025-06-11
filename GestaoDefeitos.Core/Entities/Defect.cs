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
        public required string Summary { get; set; }
        public required string Description { get; set; }
        public DefectCategory DefectCategory { get; set; }
        public DefectSeverity DefectSeverity { get; set; }
        public DefectEnvironment DefectEnvironment { get; set; }
        public required string Version { get; set; }
        public required string ExpectedBehaviour { get; set; }
        public required string ActualBehaviour { get; set; }
        public string ErrorLog { get; set; } = string.Empty;
        public List<DefectAttachment> Attachments { get; set; } = [];
        public List<Tag> Tags { get; set; } = [];
        public DateTime ExpiresIn { get; set; }
        public DefectStatus Status { get; set; } = DefectStatus.New;
        public List<DefectComment> Comments { get; set; } = [];
        public List<DefectHistory> History { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

    }
}
