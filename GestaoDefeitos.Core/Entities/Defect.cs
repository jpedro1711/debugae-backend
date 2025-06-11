using GestaoDefeitos.Domain.Enums;

namespace GestaoDefeitos.Domain.Entities
{
    public class Defect
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; }
        public Guid AssignedToContributorId { get; private set; }
        public Contributor AssignedToContributor { get; private set; }
        public string Summary { get; private set; }
        public string Description { get; private set; }
        public DefectCategory DefectCategory { get; private set; }
        public DefectSeverity DefectSeverity { get; private set; }
        public DefectEnvironment DefectEnvironment { get; private set; }
        public string Version { get; private set; }
        public string ExpectedBehaviour { get; private set; }
        public string ActualBehaviour { get; private set; }
        public string ErrorLog { get; private set; } = string.Empty;
        public List<DefectAttachment> Attachments { get; private set; } = [];
        public List<Tag> Tags { get; private set; } = [];
        public DateTime ExpiresIn { get; private set; }
        public DefectStatus Status { get; private set; }
        public List<DefectComment> Comments { get; set; } = [];
        public List<DefectHistory> History { get; set; } = [];
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Defect() { }

        public Defect(
            Guid id,
            Guid projectId,
            Project project,
            Guid assignedToContributorId,
            Contributor assignedToContributor,
            string summary,
            string description,
            DefectCategory defectCategory,
            DefectSeverity defectSeverity,
            DefectEnvironment defectEnvironment,
            string version,
            string expectedBehaviour,
            string actualBehaviour,
            DateTime expiresIn)
        {
            ArgumentNullException.ThrowIfNull(project);
            ArgumentNullException.ThrowIfNull(assignedToContributor);

            if (string.IsNullOrWhiteSpace(summary)) throw new ArgumentException("Summary is required");
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required");
            if (string.IsNullOrWhiteSpace(version)) throw new ArgumentException("Version is required");
            if (string.IsNullOrWhiteSpace(expectedBehaviour)) throw new ArgumentException("ExpectedBehaviour is required");
            if (string.IsNullOrWhiteSpace(actualBehaviour)) throw new ArgumentException("ActualBehaviour is required");

            Id = id;
            ProjectId = projectId;
            Project = project;
            AssignedToContributorId = assignedToContributorId;
            AssignedToContributor = assignedToContributor;
            Summary = summary;
            Description = description;
            DefectCategory = defectCategory;
            DefectSeverity = defectSeverity;
            DefectEnvironment = defectEnvironment;
            Version = version;
            ExpectedBehaviour = expectedBehaviour;
            ActualBehaviour = actualBehaviour;
            ErrorLog = string.Empty;
            ExpiresIn = expiresIn;
            Status = DefectStatus.New;
            CreatedAt = DateTime.UtcNow;
        }

        public void AddAttachment(DefectAttachment attachment)
        {
            ArgumentNullException.ThrowIfNull(attachment);
            Attachments.Add(attachment);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveAttachment(DefectAttachment attachment)
        {
            ArgumentNullException.ThrowIfNull(attachment);
            Attachments.Remove(attachment);
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddTag(Tag tag)
        {
            ArgumentNullException.ThrowIfNull(tag);
            if (!Tags.Contains(tag))
            {
                Tags.Add(tag);
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void RemoveTag(Tag tag)
        {
            ArgumentNullException.ThrowIfNull(tag);
            Tags.Remove(tag);
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddComment(DefectComment comment)
        {
            ArgumentNullException.ThrowIfNull(comment);
            Comments.Add(comment);
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddHistory(DefectHistory history)
        {
            ArgumentNullException.ThrowIfNull(history);
            History.Add(history);
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeStatus(DefectStatus newStatus)
        {
            if (Status == newStatus) return;
            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateErrorLog(string errorLog)
        {
            ErrorLog = errorLog ?? string.Empty;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDetails(
            string summary,
            string description,
            DefectCategory defectCategory,
            DefectSeverity defectSeverity,
            DefectEnvironment defectEnvironment,
            string version,
            string expectedBehaviour,
            string actualBehaviour,
            DateTime expiresIn)
        {
            if (string.IsNullOrWhiteSpace(summary)) throw new ArgumentException("Summary is required");
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required");
            if (string.IsNullOrWhiteSpace(version)) throw new ArgumentException("Version is required");
            if (string.IsNullOrWhiteSpace(expectedBehaviour)) throw new ArgumentException("ExpectedBehaviour is required");
            if (string.IsNullOrWhiteSpace(actualBehaviour)) throw new ArgumentException("ActualBehaviour is required");

            Summary = summary;
            Description = description;
            DefectCategory = defectCategory;
            DefectSeverity = defectSeverity;
            DefectEnvironment = defectEnvironment;
            Version = version;
            ExpectedBehaviour = expectedBehaviour;
            ActualBehaviour = actualBehaviour;
            ExpiresIn = expiresIn;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AssignToContributor(Guid contributorId, Contributor contributor)
        {
            ArgumentNullException.ThrowIfNull(contributor);
            AssignedToContributorId = contributorId;
            AssignedToContributor = contributor;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
