using GestaoDefeitos.Domain.Entities.Base.GestaoDefeitos.Domain.Pagination;
using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetDefectsByProject
{
    public record GetDefectsByProjectResponse(PagedResult<DefectsSimplifiedViewModel> Data);
}
