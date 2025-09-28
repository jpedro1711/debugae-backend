using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Entities.Base.GestaoDefeitos.Domain.Pagination;
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
                        d.Id,
                        d.Description,
                        d.Summary,
                        d.Status.ToString(),
                        d.DefectPriority.ToString(),
                        d.ExpiresIn,
                        d.CreatedAt,
                        d.DefectCategory.ToString(),
                        new ProjectSimplifiedViewModel(d.Project.Id, d.Project.Name, d.Project.Description, d.Project.CreatedAt)
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
                    Summary = d.Summary,
                    Description = d.Description,
                    Category = d.DefectCategory.ToString(),
                    Severity = d.DefectSeverity.ToString(),
                    Environment = d.DefectEnvironment.ToString(),
                    CreatedAt = d.CreatedAt,
                    Status = d.Status.ToString(),
                    Version = d.Version
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DefectsSimplifiedViewModel>> GetDefectsByContributorAsync(Guid contributorId, CancellationToken cancellationToken)
        {
            return await _context.Defects
                .Where(d => d.AssignedToContributorId == contributorId)
                .Select(d => new DefectsSimplifiedViewModel(
                        d.Id,
                        d.Description,
                        d.Summary,
                        d.Status.ToString(),
                        d.DefectPriority.ToString(),
                        d.ExpiresIn,
                        d.CreatedAt,
                        d.DefectCategory.ToString(),
                        new ProjectSimplifiedViewModel(d.Project.Id, d.Project.Name, d.Project.Description, d.Project.CreatedAt)
                       ))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Defect>> GetDefectsDataByContributorIdAsync(Guid contributorId)
        {
            return await _context.Defects
                .Where(d => d.AssignedToContributorId == contributorId)
                .ToListAsync();
        }

        public async Task<List<Defect>> GetDefectsDataByProjectIdAsync(Guid projectId)
        {
            return await _context.Defects
                .Where(d => d.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<PagedResult<DefectsSimplifiedViewModel>> GetDefectsByProjectPagedAsync(
            Guid projectId, int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _context.Defects
                .Where(d => d.ProjectId == projectId);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(d => d.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DefectsSimplifiedViewModel(
                    d.Id,
                    d.Description,
                    d.Summary,
                    d.Status.ToString(),
                    d.DefectPriority.ToString(),
                    d.ExpiresIn,
                    d.CreatedAt,
                    d.DefectCategory.ToString(),
                    new ProjectSimplifiedViewModel(d.Project.Id, d.Project.Name, d.Project.Description, d.Project.CreatedAt)
                ))
                .ToListAsync(cancellationToken);

            return new PagedResult<DefectsSimplifiedViewModel>(items, totalCount, page, pageSize);
        }

        public async Task<PagedResult<DefectsSimplifiedViewModel>> GetDefectsByContributorPagedAsync(
            Guid contributorId, int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _context.Defects
                .Where(d => d.AssignedToContributorId == contributorId);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(d => d.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DefectsSimplifiedViewModel(
                    d.Id,
                    d.Description,
                    d.Summary,
                    d.Status.ToString(),
                    d.DefectPriority.ToString(),
                    d.ExpiresIn,
                    d.CreatedAt,
                    d.DefectCategory.ToString(),
                    new ProjectSimplifiedViewModel(d.Project.Id, d.Project.Name, d.Project.Description, d.Project.CreatedAt)
                ))
                .ToListAsync(cancellationToken);

            return new PagedResult<DefectsSimplifiedViewModel>(items, totalCount, page, pageSize);
        }

        public async Task<DefectFullDetailsViewModel> GetDefectDetails(Guid defectId, CancellationToken cancellationToken)
        {
            return await _context
                    .Defects
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Where(defect => defect.Id == defectId)
                    .Select(d => new DefectFullDetailsViewModel(
                        d.Id,
                        d.Description,
                        d.Summary,
                        d.CreatedAt,
                        d.DefectHistory.Single(dh => dh.Action == DefectAction.Create).Contributor.FullName,
                        d.DefectSeverity.ToString(),
                        d.Status.ToString(),
                        d.ExpiresIn,
                        d.DefectCategory.ToString(),
                        new DefectResponsibleContributorViewModel(
                            d.AssignedToContributor.Id,
                            d.AssignedToContributor.FullName,
                            d.AssignedToContributor.Email!
                        ),
                        new DefectDetailsViewModel(
                            d.Description,
                            d.DefectEnvironment.ToString(),
                            d.ActualBehaviour,
                            d.ExpectedBehaviour,
                            d.Project.Name,
                            d.AssignedToContributor.FullName
                        ),
                        d.Comments.Select(c => new DefectCommentViewModel(
                            c.Contributor.FullName,
                            c.Content,
                            c.CreatedAt
                        )),
                        d.Attachment != null ? new DefectAttachmentViewModel(
                            d.Attachment.FileName,
                            d.Attachment.FileType,
                            d.Attachment.CreatedAt,
                            d.Attachment.UploadByUsername
                        ) : null,
                        d.RelatedDefects.Select(rd => new DefectsSimplifiedViewModel(
                            rd.RelatedDefect.Id,
                            rd.RelatedDefect.Description,
                            rd.RelatedDefect.Summary,
                            rd.RelatedDefect.Status.ToString(),
                            rd.RelatedDefect.DefectPriority.ToString(),
                            d.ExpiresIn,
                            rd.RelatedDefect.CreatedAt,
                            rd.RelatedDefect.DefectCategory.ToString(),
                            new ProjectSimplifiedViewModel(d.Project.Id, d.Project.Name, d.Project.Description, d.Project.CreatedAt)
                        )),
                        null, // it will be populated in the handler
                        d.TrelloUserStories.Select(ts => new TrelloUserStoryViewModel(
                            ts.Desc,
                            ts.ShortUrl,
                            ts.Name,
                            ts.DefectId
                        )),
                        d.ErrorLog,
                        d.Tags.Select(t => t.Description),
                        d.ProjectId
                    )).SingleAsync(cancellationToken);
        }

    }
}
