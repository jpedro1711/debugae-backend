namespace GestaoDefeitos.Domain.Entities
{
    public class DefectAttachment
    {
        public Guid Id { get; set; }
        public required string FileName { get; set; }
        public required string FileUri { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
