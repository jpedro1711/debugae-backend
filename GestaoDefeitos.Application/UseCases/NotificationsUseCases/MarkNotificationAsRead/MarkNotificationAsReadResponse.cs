namespace GestaoDefeitos.Application.UseCases.Notifications.MarkNotificationAsRead
{
    public record MarkNotificationAsReadResponse(Guid NotificationId, bool IsRead);
}
