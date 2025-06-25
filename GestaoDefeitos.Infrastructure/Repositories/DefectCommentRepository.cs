using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class DefectCommentRepository(AppDbContext context)
        : BaseRepository<DefectComment>(context), IDefectCommentRepository
    {
        public async Task<List<DefectComment>> GetCommentsByDefectIdAsync(Guid defectId, CancellationToken cancellationToken)
        {
            return await _context.DefectComments
                .Where(dc => dc.DefectId == defectId)
                .Include(dc => dc.Contributor)
                .ToListAsync(cancellationToken);
        }
    }
}
