using GestaoDefeitos.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.CreateDefect
{
    public record CreateDefectCommand : IRequest<CreateDefectResponse?>
    {
        public required string ProjectId { get; set; }
        public required string AssignedToUserEmail { get; set; }
        public required string Summary { get; set; }
        public required string Description { get; set; }
        public DefectEnvironment Environment { get; set; }
        public DefectSeverity Severity { get; set; }
        public DefectCategory Category { get; set; }
        public required string Version { get; set; }
        public required string ExpectedBehaviour { get; set; }
        public required string ActualBehaviour { get; set; }
        public required string LogTrace { get; set; }
        public DefectPriority Priority { get; set; }
        public List<Guid> DuplicatesIds { get; set; } = [];
        public IFormFile? Attachment { get; set; }
    }
}
