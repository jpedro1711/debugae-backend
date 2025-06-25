using GestaoDefeitos.Domain.Entities;

namespace GestaoDefeitos.Domain.Interfaces.Repositories
{
    public interface IDefectRelationRepository : IBaseRepository<DefectRelation>
    {
        Task<List<DefectRelation>> GetRelationsByDefectIdAsync(Guid defectId, CancellationToken cancellationToken);
    }
}
