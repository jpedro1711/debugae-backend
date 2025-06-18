using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.DetectDefectDuplicates
{
    public record DetectDefectDuplicatesResponse(int DuplicatesCount, List<DefectDuplicatesViewModel> Defects);
}
