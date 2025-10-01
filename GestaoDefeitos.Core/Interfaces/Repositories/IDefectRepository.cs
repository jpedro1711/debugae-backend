using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Entities.Base.GestaoDefeitos.Domain.Pagination;
using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Domain.Interfaces.Repositories
{
    public interface IDefectRepository : IBaseRepository<Defect>
    {
        Task<List<DefectsSimplifiedViewModel>> GetDefectsByProjectAsync(Guid projectId, CancellationToken cancellationToken);
        Task<List<DefectsSimplifiedViewModel>> GetDefectsByContributorAsync(Guid contributorId, CancellationToken cancellationToken);
        Task<List<DefectDuplicatesViewModel>> GetDefectsDuplicatedViewModelByProjectAsync(Guid projectId, CancellationToken cancellationToken);
        Task<List<Defect>> GetDefectsDataByContributorIdAsync(Guid contributorId);
        Task<PagedResult<DefectsSimplifiedViewModel>> GetDefectsByContributorPagedAsync(
            Guid contributorId, int page, int pageSize, CancellationToken cancellationToken);
        Task<PagedResult<DefectsSimplifiedViewModel>> GetDefectsByProjectPagedAsync(
            Guid projectId, int page, int pageSize, CancellationToken cancellationToken);
        Task<List<Defect>> GetDefectsDataByProjectIdAsync(Guid projectId);
        Task<DefectFullDetailsViewModel> GetDefectDetails(Guid defectId, Guid currentLoggedUserId, CancellationToken cancellationToken);
    }
}
