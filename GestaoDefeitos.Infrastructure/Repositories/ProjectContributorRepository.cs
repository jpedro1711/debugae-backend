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
    }
}
