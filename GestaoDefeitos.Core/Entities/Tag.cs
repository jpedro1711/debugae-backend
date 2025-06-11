namespace GestaoDefeitos.Domain.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
