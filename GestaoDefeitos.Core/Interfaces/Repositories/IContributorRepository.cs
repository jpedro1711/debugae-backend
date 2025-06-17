using GestaoDefeitos.Domain.Entities;

namespace GestaoDefeitos.Domain.Interfaces.Repositories
{
    public interface IContributorRepository : IBaseRepository<Contributor>
    {
        Task<List<Contributor>> GetContributorsByIdsAsync(List<string> contributorIds);
        Task<Contributor?> GetContributorByEmailAsync(string email);
    }
}
