using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class DefectHistoryRepository(AppDbContext context)
        : BaseRepository<DefectHistory>(context), IDefectHistoryRepository
    {
    }
}
