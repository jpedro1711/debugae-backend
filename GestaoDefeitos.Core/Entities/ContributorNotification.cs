namespace GestaoDefeitos.Domain.Entities
{
    public class ContributorNotification
    {
        public Guid Id { get; set; }
        public Guid ContributorId { get; set; }
        public Contributor Contributor { get; set; } = null!;
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; }
    }
}
