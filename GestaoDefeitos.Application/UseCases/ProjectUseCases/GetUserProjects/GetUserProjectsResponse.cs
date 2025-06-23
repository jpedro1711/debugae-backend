using GestaoDefeitos.Domain.Entities.Base.GestaoDefeitos.Domain.Pagination;
using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.GetUserProjects
{
    public record GetUserProjectsResponse(PagedResult<UsersProjectViewModel> Data);
}
