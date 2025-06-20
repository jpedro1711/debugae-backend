using GestaoDefeitos.Domain.Entities;

namespace GestaoDefeitos.Domain.Interfaces.Repositories
{
    public interface IDefectAttachmentRepository : IBaseRepository<DefectAttachment>
    {
        Task<DefectAttachment?> GetAttachmentByDefectIdAsync(Guid DefectId);
    }
}
