using GestaoDefeitos.Domain.Enums;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.UpdateDefectStatus
{
    public record UpdateDefectStatusResponse(Guid DefectId, DefectStatus DefectStatus);
}
