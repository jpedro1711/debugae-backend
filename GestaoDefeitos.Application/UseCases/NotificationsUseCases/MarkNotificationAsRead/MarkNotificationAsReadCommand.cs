using MediatR;

namespace GestaoDefeitos.Application.UseCases.Notifications.MarkNotificationAsRead
{
    public record MarkNotificationAsReadCommand(Guid NotificationId) : IRequest<MarkNotificationAsReadResponse?>;
}
