namespace GestaoDefeitos.Domain.Entities
{
    public class DefectComment
    {
        public Guid Id { get; private set; }
        public string Content { get; private set; }
        public Guid ContributorId { get; private set; }
        public Contributor Contributor { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private DefectComment() { }

        public DefectComment(Guid id, string content, Guid contributorId, Contributor contributor)
        {
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("Content is required");
            ArgumentNullException.ThrowIfNull(contributor);

            Id = id;
            Content = content;
            ContributorId = contributorId;
            Contributor = contributor;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("Content is required");
            Content = content;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

