using GestaoDefeitos.Domain.Entities.RelationEntities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class ProjectContributorRepository(AppDbContext context)
        : BaseRepository<ProjectContributor>(context), 
          IProjectContributorRepository
    {
    }
}
