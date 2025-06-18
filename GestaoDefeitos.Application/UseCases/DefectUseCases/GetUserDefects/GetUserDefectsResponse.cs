using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetUserDefects
{
    public record GetUserDefectsResponse(List<DefectsSimplifiedViewModel> UserDefects);
}
