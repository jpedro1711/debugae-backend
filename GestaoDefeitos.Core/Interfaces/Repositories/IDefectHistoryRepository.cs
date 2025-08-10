using GestaoDefeitos.Domain.Entities.Events;
using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Domain.Interfaces.Repositories
{
    public interface IDefectHistoryRepository : IBaseRepository<DefectChangeEvent>
    {
        Task<DefectChangeEvent?> GetDefectCreateHistoryByDefectIdAsync(Guid defectId, CancellationToken cancellationToken);
        Task<List<DefectHistoryViewModel>> GetDefectHistoryByDefectIdAsync(Guid defectId, CancellationToken cancellationToken);
    }
}
