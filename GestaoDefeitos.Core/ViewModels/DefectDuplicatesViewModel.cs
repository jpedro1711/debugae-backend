using GestaoDefeitos.Domain.Enums;

namespace GestaoDefeitos.Domain.ViewModels
{
    public class DefectDuplicatesViewModel
    {
        public Guid DefectId { get; set; }
        public required string ProjectId { get; set; }
        public required string AssignedToUserId { get; set; }
        public required string Summary { get; set; }
        public required string Description { get; set; }
        public required string Environment { get; set; }
        public required string Severity { get; set; }
        public required string Category { get; set; }
        public required string Version { get; set; }
        public int Score { get; set; }
    }
}
