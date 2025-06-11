using GestaoDefeitos.Domain.Enums;

namespace GestaoDefeitos.Domain.Entities
{
    public class DefectHistory
    {
        public Guid Id { get; private set; }
        public Guid ContributorId { get; private set; }
        public Contributor Contributor { get; private set; }
        public DefectAction Action { get; private set; }
        public string OldMetadataJson { get; private set; }
        public string NewMetadataJson { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private DefectHistory() { }

        public DefectHistory(
            Guid id,
            Guid contributorId,
            Contributor contributor,
            DefectAction action,
            string oldMetadataJson,
            string newMetadataJson)
        {
            ArgumentNullException.ThrowIfNull(contributor);

            if (string.IsNullOrWhiteSpace(oldMetadataJson)) throw new ArgumentException("OldMetadataJson is required");

            Id = id;
            ContributorId = contributorId;
            Contributor = contributor;
            Action = action;
            OldMetadataJson = oldMetadataJson;
            NewMetadataJson = newMetadataJson;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateMetadata(string oldMetadataJson, string newMetadataJson)
        {
            if (string.IsNullOrWhiteSpace(oldMetadataJson)) throw new ArgumentException("OldMetadataJson is required");
            if (string.IsNullOrWhiteSpace(newMetadataJson)) throw new ArgumentException("NewMetadataJson is required");

            OldMetadataJson = oldMetadataJson;
            NewMetadataJson = newMetadataJson;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
