using GestaoDefeitos.Domain.Enums;

namespace GestaoDefeitos.Domain.Entities
{
    public class DefectHistory
    {
        public Guid Id { get; set; }
        public Guid DefectId { get; set; }
        public Guid ContributorId { get; set; }
        public Contributor Contributor { get; set; } = null!;
        public DefectAction Action { get; set; }
        public string OldMetadataJson { get; set; } = null!;
        public string NewMetadataJson { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
