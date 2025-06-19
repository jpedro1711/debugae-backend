using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GestaoDefeitos.Infrastructure.Repositories
{
    public class ContributorNotificationRepository(AppDbContext context)         
        : BaseRepository<ContributorNotification>(context), IContributorNotificationRepository
    {
        public async Task<List<ContributorNotification>> GetNotificationsByContributorIdAsync(Guid contributorId)
        {
            return await _context.ContributorNotifications
                .Where(n => n.ContributorId == contributorId)
                .ToListAsync();
        }

        public async Task MarkNotificationAsReadAsync(Guid notificationId)
        {
            var notification = await _context.ContributorNotifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                notification.LastUpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
