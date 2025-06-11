namespace GestaoDefeitos.Domain.Entities
{
    public class DefectComment
    {
        public Guid Id { get; set; }
        public required string Content { get; set; }
        public Guid ContributorId { get; set; }
        public Contributor Contributor { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
