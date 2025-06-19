namespace GestaoDefeitos.Domain.Entities
{
    public class DefectAttachment
    {
        public Guid Id { get; set; }
        public Guid DefectId { get; set; }
        public Defect Defect { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public string UploadByUsername { get; set; } = null!;
        public byte[] FileContent { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
