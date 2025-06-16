namespace GestaoDefeitos.Domain.Entities
{
    public class DefectAttachment
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = null!;
        public string FileUri { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
