using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.Notifications.GetCurrentLoggedUserNotifications
{
    public class GetCurrentUserNotificationsHandler
        (
            AuthenticationContextAcessor authenticationContextAcessor,
            IContributorNotificationRepository contributorNotificationRepository
        ) : IRequestHandler<GetCurrentLoggedUserNotificationsQuery, GetCurrentLoggedUserNotificationsResponse?>
    {
        public async Task<GetCurrentLoggedUserNotificationsResponse?> Handle(GetCurrentLoggedUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            var loggedUserId = authenticationContextAcessor.GetCurrentLoggedUserId();

            var notifications = await contributorNotificationRepository.GetNotificationsByContributorIdAsync(loggedUserId);

            if (notifications is null)
                return null;

            return new GetCurrentLoggedUserNotificationsResponse(notifications);
        }
    }
}
