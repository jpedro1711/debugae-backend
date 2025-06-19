using MediatR;

namespace GestaoDefeitos.Application.UseCases.Notifications.GetCurrentLoggedUserNotifications
{
    public record GetCurrentLoggedUserNotificationsQuery() : IRequest<GetCurrentLoggedUserNotificationsResponse?>;
}
