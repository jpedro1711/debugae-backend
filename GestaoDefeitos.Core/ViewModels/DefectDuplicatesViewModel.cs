using GestaoDefeitos.Domain.Enums;

namespace GestaoDefeitos.Domain.ViewModels
{
    public class DefectDuplicatesViewModel
    {
        public required string ProjectId { get; set; }
        public required string AssignedToUserId { get; set; }
        public required string Summary { get; set; }
        public required string Description { get; set; }
        public DefectEnvironment Environment { get; set; }
        public DefectSeverity Severity { get; set; }
        public DefectCategory Category { get; set; }
        public required string Version { get; set; }
        public int Score { get; set; }
    }
}
