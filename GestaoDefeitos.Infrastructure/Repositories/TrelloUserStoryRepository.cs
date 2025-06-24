using GestaoDefeitos.Application.TrelloIntegration;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class TrelloUserStoryRepository(AppDbContext context)
        : BaseRepository<TrelloUserStory>(context), ITrelloUserStoryRepository
    {
        public async Task<List<TrelloUserStory>?> GetUserStoriesByDefectId(Guid defectId)
        {
            return await context.TrelloUserStories
                .Where(us => us.DefectId == defectId)
                .ToListAsync();
        }
    }
}
