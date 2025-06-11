namespace GestaoDefeitos.Domain.Entities
{
    public class ContributorNotification
    {
        public Guid Id { get; private set; }
        public Guid ContributorId { get; private set; }
        public Contributor Contributor { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }

        private ContributorNotification() { }

        public ContributorNotification(Guid id, Guid contributorId, Contributor contributor, string content)
        {
            ArgumentNullException.ThrowIfNull(contributor);
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("Content is required");

            Id = id;
            ContributorId = contributorId;
            Contributor = contributor;
            Content = content;
            CreatedAt = DateTime.UtcNow;
            LastUpdatedAt = CreatedAt;
        }

        public void UpdateContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("Content is required");
            Content = content;
            LastUpdatedAt = DateTime.UtcNow;
        }
    }
}
