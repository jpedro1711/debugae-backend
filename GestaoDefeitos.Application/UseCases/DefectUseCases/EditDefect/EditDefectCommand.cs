using GestaoDefeitos.Domain.Enums;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.EditDefect
{
    public record EditDefectCommand(
            Guid DefectId,
            string NewDescription,
            DefectEnvironment NewEnvironment,
            DefectSeverity NewSeverity,
            DefectStatus NewStatus,
            DefectCategory NewCategory,
            string NewCurrentBehaviour,
            string NewExpectedBehaviour,
            string NewStackTrace,
            string NewAssignedToContributorEmail
        ) : IRequest<EditDefectResponse>;
}
