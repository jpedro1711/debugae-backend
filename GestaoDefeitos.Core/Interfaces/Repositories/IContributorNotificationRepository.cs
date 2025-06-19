using GestaoDefeitos.Domain.Entities;

namespace GestaoDefeitos.Domain.Interfaces.Repositories
{
    public interface IContributorNotificationRepository : IBaseRepository<ContributorNotification>
    {
        Task<List<ContributorNotification>> GetNotificationsByContributorIdAsync(Guid contributorId);
        Task MarkNotificationAsReadAsync(Guid notificationId);
    }
}
