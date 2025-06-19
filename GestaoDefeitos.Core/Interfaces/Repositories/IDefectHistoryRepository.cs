using GestaoDefeitos.Domain.Entities;

namespace GestaoDefeitos.Domain.Interfaces.Repositories
{
    public interface IDefectHistoryRepository : IBaseRepository<DefectHistory>
    {
        Task<DefectHistory?> GetDefectCreateHistoryByDefectIdAsync(Guid defectId, CancellationToken cancellationToken);
    }
}
