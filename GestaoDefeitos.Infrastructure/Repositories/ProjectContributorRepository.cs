using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Entities.Base.GestaoDefeitos.Domain.Pagination;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Domain.ViewModels;
using GestaoDefeitos.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class ProjectContributorRepository(AppDbContext context)
        : BaseRepository<ProjectContributor>(context), 
          IProjectContributorRepository
    {
        public async Task<PagedResult<UsersProjectViewModel>> GetProjectContributorsByUserIdAsync(Guid userId, int page, int pageSize)
        {
            var query = _context.ProjectContributors
                .Where(pc => pc.ContributorId == userId)
                .Include(pc => pc.Project);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(pc => pc.Project.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(pc => new UsersProjectViewModel
                (
                    pc.Project.Id.ToString(),
                    pc.Project.Name,
                    pc.Project.Description,
                    pc.Project.ProjectContributors.Count,
                    pc.Role.ToString(),
                    pc.Project.ProjectContributors
                        .Select(c => new ProjectColaboratorViewModel
                        (
                            c.ContributorId,
                            c.Contributor.FullName,
                            c.Role.ToString(),
                            c.Contributor.Email
                        )).ToList()
                ))
                .ToListAsync();

            return new PagedResult<UsersProjectViewModel>(items, totalCount, page, pageSize);
        }

        public async Task<List<UsersProjectViewModel>> GetAllProjectContributorsByUserIdAsync(Guid userId)
        {
            return await _context.ProjectContributors
                .Where(pc => pc.ContributorId == userId)
                .Include(pc => pc.Project)
                .Select(pc => new UsersProjectViewModel
                (
                    pc.Project.Id.ToString(),
                    pc.Project.Name,
                    pc.Project.Description,
                    pc.Project.ProjectContributors.Count,
                    pc.Role.ToString(),
                    pc.Project.ProjectContributors
                        .Select(c => new ProjectColaboratorViewModel
                        (
                            c.ContributorId,
                            c.Contributor.FullName,
                            c.Role.ToString(),
                            c.Contributor.Email
                        )).ToList()
                ))
                .ToListAsync();
        }

        public async Task<bool> IsUserOnProject(Guid userId, Guid projectId)
        {
            return await _context.ProjectContributors
                .AnyAsync(pc => pc.ContributorId == userId && pc.ProjectId == projectId);
        }

        public async Task<ProjectContributor?> GetByProjectAndUserIds(Guid projectId, Guid userId)
        {
            return await _context.ProjectContributors
                .FirstOrDefaultAsync(pc => pc.ProjectId == projectId && pc.ContributorId == userId);
        }

        public async Task RemoveProjectContributor(ProjectContributor projectContributor)
        {
            _context.ProjectContributors.Remove(projectContributor);
            await _context.SaveChangesAsync();
        }
    }
}
