using GestaoDefeitos.Domain.Entities;

namespace GestaoDefeitos.Domain.Interfaces.Repositories
{
    public interface IDefectMailLetterRepository : IBaseRepository<DefectMailLetter>
    {
        Task AddToMailLetter(Guid DefectId, Guid ContributorId);
        Task RemoveFromMailLetter(Guid DefectId, Guid ContributorId);
    }
}
