namespace GestaoDefeitos.Domain.Entities
{
    public class ContributorNotification
    {
        public Guid Id { get; set; }
        public Guid ContributorId { get; set; }
        public Contributor Contributor { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; }
    }
}
