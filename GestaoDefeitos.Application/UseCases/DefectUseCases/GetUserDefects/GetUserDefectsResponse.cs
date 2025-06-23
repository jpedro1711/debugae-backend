using GestaoDefeitos.Domain.Entities.Base.GestaoDefeitos.Domain.Pagination;
using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetUserDefects
{
    public record GetUserDefectsResponse(PagedResult<DefectsSimplifiedViewModel> Data);
}
