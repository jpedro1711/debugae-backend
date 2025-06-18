using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Domain.ViewModels;
using GestaoDefeitos.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class DefectRepository(AppDbContext context)
        : BaseRepository<Defect>(context), IDefectRepository
    {
        public async Task<List<DefectsSimplifiedViewModel>> GetDefectsByProjectAsync(Guid projectId, CancellationToken cancellationToken)
        {
            return await _context.Defects
                .Where(d => d.ProjectId == projectId)
                .Select(d => new DefectsSimplifiedViewModel(
                        d.Id.ToString(),
                        d.Description,
                        d.Summary,
                        d.Status.ToString(),
                        d.DefectPriority.ToString(),
                        d.CreatedAt
                       ))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DefectsSimplifiedViewModel>> GetDefectsByContributorAsync(Guid contributorId, CancellationToken cancellationToken)
        {
            return await _context.Defects
                .Where(d => d.AssignedToContributorId == contributorId)
                .Select(d => new DefectsSimplifiedViewModel(
                        d.Id.ToString(),
                        d.Description,
                        d.Summary,
                        d.Status.ToString(),
                        d.DefectPriority.ToString(),
                        d.CreatedAt
                       ))
                .ToListAsync(cancellationToken);
        }
    }
}
