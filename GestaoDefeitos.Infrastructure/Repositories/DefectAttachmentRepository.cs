using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class DefectAttachmentRepository(AppDbContext context)
        : BaseRepository<DefectAttachment>(context), IDefectAttachmentRepository
    {
        public async Task<DefectAttachment?> GetAttachmentByDefectIdAsync(Guid DefectId)
        {
            return await context.DefectAttachments.SingleOrDefaultAsync(da => da.DefectId == DefectId);
        }
    }
}
