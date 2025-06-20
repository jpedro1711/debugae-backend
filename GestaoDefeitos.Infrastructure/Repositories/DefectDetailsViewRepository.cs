using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class DefectDetailsViewRepository(AppDbContext context)
        : BaseRepository<DefectDetailsView>(context), IDefectDetailsViewRepository
    {
        public async Task<DefectDetailsView?> GetDefectDetails(Guid DefectId, CancellationToken cancellationToken)
        {
            return await _context.DefectDetailsView
                .Where(v => v.Id == DefectId)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
