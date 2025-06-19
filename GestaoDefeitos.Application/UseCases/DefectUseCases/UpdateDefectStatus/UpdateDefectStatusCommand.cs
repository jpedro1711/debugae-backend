using GestaoDefeitos.Domain.Enums;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.UpdateDefectStatus
{
    public record UpdateDefectStatusCommand(Guid DefectId, DefectStatus NewStatus) : IRequest<UpdateDefectStatusResponse?>;
}
