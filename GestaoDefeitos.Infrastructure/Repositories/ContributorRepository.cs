using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class ContributorRepository(AppDbContext context)
        : BaseRepository<Contributor>(context), IContributorRepository
    {
        public async Task<List<Contributor>> GetContributorsByIdsAsync(List<string> contributorIds)
        {
            return await _context.Users
                .Where(c => contributorIds.Contains(c.Id.ToString()))
                .ToListAsync();
        }

        public async Task<Contributor?> GetContributorByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}
