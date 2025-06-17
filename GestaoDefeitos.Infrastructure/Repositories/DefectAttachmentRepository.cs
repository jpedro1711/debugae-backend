using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class DefectAttachmentRepository(AppDbContext context)
        : BaseRepository<DefectAttachment>(context), IDefectAttachmentRepository
    {
    }
}
