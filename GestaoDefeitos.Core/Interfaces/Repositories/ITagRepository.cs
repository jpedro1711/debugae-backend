using GestaoDefeitos.Domain.Entities;

namespace GestaoDefeitos.Domain.Interfaces.Repositories
{
    public interface ITagRepository : IBaseRepository<Tag>
    {
        Task<Tag?> GetTagByValueAsync(string tagValue, CancellationToken cancellationToken);
        Task<bool> TagExistsAsync(string tagValue, CancellationToken cancellationToken);
        Task<List<Tag>> GetTagsByDefect(Guid defectId);
    }
}
