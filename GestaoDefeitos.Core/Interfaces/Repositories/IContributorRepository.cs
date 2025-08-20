using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Domain.Interfaces.Repositories
{
    public interface IContributorRepository : IBaseRepository<Contributor>
    {
        Task<List<Contributor>> GetContributorsByIdsAsync(List<string> contributorIds);
        Task<Contributor?> GetContributorByEmailAsync(string email);
        Task<List<ColaboratorViewModel>> GetAllColaborators();
    }
}
