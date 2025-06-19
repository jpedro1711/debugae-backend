using GestaoDefeitos.Domain.Entities;

namespace GestaoDefeitos.Domain.Interfaces.Repositories
{
    public interface IProjectRepository : IBaseRepository<Project>
    {
        Task<Project?> GetProjectDetailsAsync(Guid projectId, CancellationToken cancellationToken);
    }
}
