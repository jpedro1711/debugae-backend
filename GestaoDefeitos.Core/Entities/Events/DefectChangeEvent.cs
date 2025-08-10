using GestaoDefeitos.Domain.Enums;

namespace GestaoDefeitos.Domain.Entities.Events
{
    public class DefectChangeEvent
    {
        public Guid Id { get; set; }
        public Guid DefectId { get; set; }
        public Defect Defect { get; set; } = null!;
        public Guid ContributorId { get; set; }
        public Contributor Contributor { get; set; } = null!;
        public DefectAction Action { get; set; }
        public string? Field { get; set; } = null!;
        public string? OldValue { get; set; } = null!;
        public string? NewValue { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
