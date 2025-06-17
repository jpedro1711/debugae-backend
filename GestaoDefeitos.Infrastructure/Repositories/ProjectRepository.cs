using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class ProjectRepository(AppDbContext context)
        : BaseRepository<Project>(context), IProjectRepository
    {
    }
}
