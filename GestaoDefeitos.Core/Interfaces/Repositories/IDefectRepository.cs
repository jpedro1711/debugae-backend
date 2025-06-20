using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Domain.Interfaces.Repositories
{
    public interface IDefectRepository : IBaseRepository<Defect>
    {
        Task<List<DefectsSimplifiedViewModel>> GetDefectsByProjectAsync(Guid projectId, CancellationToken cancellationToken);
        Task<List<DefectsSimplifiedViewModel>> GetDefectsByContributorAsync(Guid contributorId, CancellationToken cancellationToken);
        Task<List<DefectDuplicatesViewModel>> GetDefectsDuplicatedViewModelByProjectAsync(Guid projectId, CancellationToken cancellationToken);
    }
}
