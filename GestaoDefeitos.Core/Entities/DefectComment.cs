namespace GestaoDefeitos.Domain.Entities
{
    public class DefectComment
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public Guid ContributorId { get; set; }
        public Contributor Contributor { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

