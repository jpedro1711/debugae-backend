using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class DefectRelationRepository(AppDbContext context)
        : BaseRepository<DefectRelation>(context), IDefectRelationRepository
    {
        public async Task<List<DefectRelation>> GetRelationsByDefectIdAsync(Guid defectId, CancellationToken cancellationToken)
        {
            return await _context.DefectRelations
                .Where(dr => dr.DefectId == defectId)
                .Include(dr => dr.RelatedDefect)
                .ToListAsync(cancellationToken);
        }
    }
}
