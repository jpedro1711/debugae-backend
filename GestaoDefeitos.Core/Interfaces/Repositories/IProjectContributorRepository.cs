using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Entities.Base.GestaoDefeitos.Domain.Pagination;
using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Domain.Interfaces.Repositories
{
    public interface IProjectContributorRepository : IBaseRepository<ProjectContributor>
    {
        Task<PagedResult<UsersProjectViewModel>> GetProjectContributorsByUserIdAsync(Guid userId, int page, int pageSize);
        Task<bool> IsUserOnProject(Guid userId, Guid projectId);
        Task<ProjectContributor?> GetByProjectAndUserIds(Guid projectId, Guid userId);
        Task RemoveProjectContributor(ProjectContributor projectContributor);
    }
}
