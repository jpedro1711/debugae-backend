using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GestaoDefeitos.Application.UseCases.Notifications.GetCurrentLoggedUserNotifications
{
    public class GetCurrentUserNotificationsHandler
        (
            IHttpContextAccessor httpContextAccessor,
            IContributorNotificationRepository contributorNotificationRepository
        ) : IRequestHandler<GetCurrentLoggedUserNotificationsQuery, GetCurrentLoggedUserNotificationsResponse?>
    {
        public async Task<GetCurrentLoggedUserNotificationsResponse?> Handle(GetCurrentLoggedUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            var loggedUserId = Guid.Parse(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var notifications = await contributorNotificationRepository.GetNotificationsByContributorIdAsync(loggedUserId);

            if (notifications is null)
                return null;

            return new GetCurrentLoggedUserNotificationsResponse(notifications);
        }
    }
}
