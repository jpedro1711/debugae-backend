using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;
using GestaoDefeitos.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using GestaoDefeitos.Domain.Entities.Events;
using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class DefectHistoryRepository(AppDbContext context)
        : BaseRepository<DefectChangeEvent>(context), IDefectHistoryRepository
    {
        public async Task<DefectChangeEvent?> GetDefectCreateHistoryByDefectIdAsync(Guid defectId, CancellationToken cancellationToken)
        {
            return await _context.DefectHistory
                .Where(history => history.DefectId == defectId && history.Action == DefectAction.Create)
                .Include(dh => dh.Contributor)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<DefectHistoryViewModel>> GetDefectHistoryByDefectIdAsync(Guid defectId, CancellationToken cancellationToken)
        {
            return await _context.DefectHistory
                .AsNoTracking()
                .Where(history => history.DefectId == defectId)
                .OrderBy(h => h.CreatedAt)
                .Select(h => new DefectHistoryViewModel
                (
                    h.Action.ToString(),
                    h.Field,
                    h.OldValue,
                    h.NewValue,
                    h.Contributor.FullName,
                    h.CreatedAt
                ))
                .ToListAsync(cancellationToken);
        }
    }
}
