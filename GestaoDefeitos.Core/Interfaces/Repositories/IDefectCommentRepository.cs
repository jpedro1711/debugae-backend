using GestaoDefeitos.Domain.Entities;

namespace GestaoDefeitos.Domain.Interfaces.Repositories
{
    public interface IDefectCommentRepository : IBaseRepository<DefectComment>
    {
        Task<List<DefectComment>> GetCommentsByDefectIdAsync(Guid defectId, CancellationToken cancellationToken);
    }
}
