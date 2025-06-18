using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetDefectsByProject
{
    public record GetDefectsByProjectResponse(List<DefectsSimplifiedViewModel> ProjectDefects);
}
