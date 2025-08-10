using GestaoDefeitos.Application.TrelloIntegration;

namespace GestaoDefeitos.Domain.Interfaces.Repositories
{
    public interface ITrelloUserStoryRepository : IBaseRepository<TrelloUserStory>
    {
        Task<List<TrelloUserStory>?> GetUserStoriesByDefectId(Guid defectId);
    }
}
