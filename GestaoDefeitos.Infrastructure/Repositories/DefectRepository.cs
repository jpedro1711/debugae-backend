using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class DefectRepository(AppDbContext context)
        : BaseRepository<Defect>(context), IDefectRepository
    {
    }
}
