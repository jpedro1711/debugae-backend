using GestaoDefeitos.Domain.Entities;

namespace GestaoDefeitos.Domain.Interfaces.Repositories
{
    public interface IDefectDetailsViewRepository :  IBaseRepository<DefectDetailsView>
    {
        Task<DefectDetailsView?> GetDefectDetails(Guid DefectId, CancellationToken cancellationToken);
    }
}
