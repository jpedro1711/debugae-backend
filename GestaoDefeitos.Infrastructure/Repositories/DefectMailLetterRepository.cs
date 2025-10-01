using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class DefectMailLetterRepository(AppDbContext context)
        : BaseRepository<DefectMailLetter>(context), IDefectMailLetterRepository
    {
        public async Task AddToMailLetter(Guid DefectId, Guid ContributorId)
        {
            await context.DefectMailLetters.AddAsync(new DefectMailLetter
            {
                DefectId = DefectId,
                ContributorId = ContributorId
            });

            await context.SaveChangesAsync();
        }
        
        public async Task RemoveFromMailLetter(Guid DefectId, Guid ContributorId)
        {
            var entity = await context.DefectMailLetters.FirstOrDefaultAsync(x => x.DefectId == DefectId && x.ContributorId == ContributorId);

            if (entity != null)
            {
                context.DefectMailLetters.Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<DefectMailLetter?> GetByCompositeIdAsync(Guid DefectId, Guid ContributorId)
        {
            return await context.DefectMailLetters.FirstOrDefaultAsync(x => x.DefectId == DefectId && x.ContributorId == ContributorId);
        }
    }
}
