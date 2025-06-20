using GestaoDefeitos.Domain.Entities;
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
        public async Task<List<UsersProjectViewModel>> GetProjectContributorsByUserIdAsync(Guid userId)
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
                    pc.Role.ToString()
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
