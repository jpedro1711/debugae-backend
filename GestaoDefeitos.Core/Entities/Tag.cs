namespace GestaoDefeitos.Domain.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public Guid DefectId { get; set; }
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
