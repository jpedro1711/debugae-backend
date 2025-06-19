using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Enums;
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

        public async Task<List<DefectDuplicatesViewModel>> GetDefectsDuplicatedViewModelByProjectAsync(Guid projectId, CancellationToken cancellationToken)
        {
            return await _context.Defects
                .Where(d => d.ProjectId == projectId)
                .Select(d => new DefectDuplicatesViewModel
                {
                    DefectId = d.Id,
                    ProjectId = d.ProjectId.ToString(),
                    AssignedToUserId = d.AssignedToContributorId.ToString(),
                    Summary = d.Summary,
                    Description = d.Description,
                    Category = d.DefectCategory,
                    Severity = d.DefectSeverity,
                    Environment = d.DefectEnvironment,
                    Version = d.Version
                })
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

        public async Task<DefectAllDetailsViewModel?> GetDefectDetailsProjectionAsync(Guid defectId, CancellationToken cancellationToken)
        {
            var history = await _context.DefectHistory
                .Where(h => h.DefectId == defectId && h.Action == DefectAction.Create)
                .Include(h => h.Contributor)
                .FirstOrDefaultAsync(cancellationToken);

            var response = await _context.Defects
                .Where(d => d.Id == defectId)
                .Select(d => new DefectAllDetailsViewModel(
                    d.Id,
                    d.Description,
                    d.Summary,
                    d.CreatedAt,
                    (history != null) ? history.Contributor.Firstname + " " + history.Contributor.Lastname : null, 
                    d.DefectSeverity.ToString(),
                    d.Status.ToString(),
                    d.ExpiresIn,
                    d.DefectCategory.ToString(),
                    new DefectResponsibleContributorViewModel(
                        d.AssignedToContributor.Id,
                        d.AssignedToContributor.Firstname + " " + d.AssignedToContributor.Lastname
                    ),
                    new DefectDetailsViewModel(
                        d.Description,
                        d.DefectEnvironment.ToString(),
                        d.ActualBehaviour,
                        d.ExpectedBehaviour,
                        d.Project.Name,
                        d.AssignedToContributor.Firstname + " " + d.AssignedToContributor.Lastname
                    ),
                    d.Comments.Select(c => new DefectCommentViewModel(
                        c.Contributor.Firstname + " " + c.Contributor.Lastname,
                        c.Content,
                        c.CreatedAt
                    )).ToList(),
                    d.Attachment != null
                        ? new DefectAttachmentViewModel(
                            d.Attachment.FileName,
                            d.Attachment.FileType,
                            d.Attachment.CreatedAt
                        )
                        : null,
                    d.RelatedDefects.Select(rd => new DefectsSimplifiedViewModel(
                        rd.RelatedDefect.Id.ToString(),
                        rd.RelatedDefect.Description,
                        rd.RelatedDefect.Summary,
                        rd.RelatedDefect.Status.ToString(),
                        rd.RelatedDefect.DefectPriority.ToString(),
                        rd.RelatedDefect.CreatedAt
                    )).ToList()
                ))
                .FirstOrDefaultAsync(cancellationToken);

            if (response is null)
                return null;

            return response;
        }

    }
}
