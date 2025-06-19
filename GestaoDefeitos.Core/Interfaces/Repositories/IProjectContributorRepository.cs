using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Domain.Interfaces.Repositories
{
    public interface IProjectContributorRepository : IBaseRepository<ProjectContributor>
    {
        Task<List<UsersProjectViewModel>> GetProjectContributorsByUserIdAsync(Guid userId);
        Task<bool> IsUserOnProject(Guid userId, Guid projectId);
    }
}
