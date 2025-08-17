using GestaoDefeitos.Domain.Enums;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.DetectDefectDuplicates
{
    public record DetectDefectDuplicatesCommand : IRequest<DetectDefectDuplicatesResponse>
    {
        public required string ProjectId { get; set; }
        public required string Summary { get; set; }
        public required string Description { get; set; }
        public DefectEnvironment Environment { get; set; }
        public DefectSeverity Severity { get; set; }
        public DefectCategory Category { get; set; }
        public DefectStatus Status { get; set; }
        public required string Version { get; set; }
    }
}
