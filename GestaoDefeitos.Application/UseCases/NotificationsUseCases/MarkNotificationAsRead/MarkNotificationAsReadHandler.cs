using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.Notifications.MarkNotificationAsRead
{
    public class MarkNotificationAsReadHandler
        (
            IContributorNotificationRepository notificationRepository
        ) : IRequestHandler<MarkNotificationAsReadCommand, MarkNotificationAsReadResponse?>
    {
        public async Task<MarkNotificationAsReadResponse?> Handle(MarkNotificationAsReadCommand command, CancellationToken cancellationToken)
        {
            var notification = await notificationRepository.GetByIdAsync(command.NotificationId);

            if (notification == null)
                return null;

            await notificationRepository.MarkNotificationAsReadAsync(notification.Id);

            return new MarkNotificationAsReadResponse(notification.Id, notification.IsRead);
        }
    }
}
