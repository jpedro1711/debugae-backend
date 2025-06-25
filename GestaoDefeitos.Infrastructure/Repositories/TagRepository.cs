using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class TagRepository(AppDbContext context)
        : BaseRepository<Tag>(context), ITagRepository
    {
        public async Task<Tag?> GetTagByValueAsync(string tagValue, CancellationToken cancellationToken)
        {
            return await _context.Tags
                .FirstOrDefaultAsync(t => t.Description.Equals(tagValue), cancellationToken);
        }

        public async Task<bool> TagExistsAsync(string tagValue, CancellationToken cancellationToken)
        {
            return await _context.Tags
                .AnyAsync(t => t.Description.Equals(tagValue), cancellationToken);
        }

        public async Task<List<Tag>> GetTagsByDefect(Guid defectId)
        {
            return await _context.Tags
                .Where(t => t.DefectId == defectId)
                .ToListAsync();
        }
    }
}
