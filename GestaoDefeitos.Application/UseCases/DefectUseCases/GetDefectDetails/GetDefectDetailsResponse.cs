using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetDefectDetails
{
    public record GetDefectDetailsResponse(
            DefectFullDetailsViewModel DefectData
        );
}
