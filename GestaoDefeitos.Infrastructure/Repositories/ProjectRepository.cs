using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class ProjectRepository(AppDbContext context)
        : BaseRepository<Project>(context), IProjectRepository
    {
        public async Task<Project?> GetProjectDetailsAsync(Guid projectId, CancellationToken cancellationToken)
        {
            return await _context.Projects
                .Where(project => project.Id == projectId)
                .Include(project => project.ProjectContributors)
                    .ThenInclude(contributor => contributor.Contributor)
                .Include(project => project.Defects)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<Project?> GetProjectByName(string projectName, CancellationToken cancellationToken) =>
            await _context.Projects
                .FirstOrDefaultAsync(project => project.Name == projectName, cancellationToken);
    }
}
