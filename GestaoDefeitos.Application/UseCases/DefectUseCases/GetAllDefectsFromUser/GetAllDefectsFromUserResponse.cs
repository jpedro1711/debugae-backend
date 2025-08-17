using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetAllDefectsFromUser
{
    public record GetAllDefectsFromUserResponse(List<DefectsSimplifiedViewModel> Data);
}
