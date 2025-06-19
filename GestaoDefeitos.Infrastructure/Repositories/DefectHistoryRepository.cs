using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;
using GestaoDefeitos.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class DefectHistoryRepository(AppDbContext context)
        : BaseRepository<DefectHistory>(context), IDefectHistoryRepository
    {
        public async Task<DefectHistory?> GetDefectCreateHistoryByDefectIdAsync(Guid defectId, CancellationToken cancellationToken)
        {
            return await _context.DefectHistory
                .Where(history => history.DefectId == defectId && history.Action == DefectAction.Create)
                .Include(dh => dh.Contributor)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<DefectHistory>> GetDefectHistoryByDefectIdAsync(Guid defectId, CancellationToken cancellationToken)
        {
            return await _context.DefectHistory
                .Where(history => history.DefectId == defectId)
                .Include(dh => dh.Contributor)
                .OrderByDescending(dh => dh.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
