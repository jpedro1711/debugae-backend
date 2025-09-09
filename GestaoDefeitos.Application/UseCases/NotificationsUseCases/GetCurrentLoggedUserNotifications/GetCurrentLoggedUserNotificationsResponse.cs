using GestaoDefeitos.Domain.Entities;

namespace GestaoDefeitos.Application.UseCases.Notifications.GetCurrentLoggedUserNotifications
{
    public record GetCurrentLoggedUserNotificationsResponse(List<ContributorNotification> Notifications);
}
