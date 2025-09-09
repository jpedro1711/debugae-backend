using GestaoDefeitos.Application.UseCases.Notifications.GetCurrentLoggedUserNotifications;
using GestaoDefeitos.Application.UseCases.Notifications.MarkNotificationAsRead;
using MediatR;

namespace GestaoDefeitos.WebApi.Endpoints
{
    public static class NotificationEndpoints
    {
        public static IEndpointRouteBuilder MapNotificationEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/notifications")
                .WithTags("Notifications");

            group.MapMarkNotificationAsReadEndpoint();
            group.MapGetUserNotifications();

            return endpoints;
        }

        public static RouteGroupBuilder MapMarkNotificationAsReadEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/markAsRead", async (
                MarkNotificationAsReadCommand command,
                IMediator mediator) =>
            {
                var readNotificationResponse = await mediator.Send(command);

                return (readNotificationResponse is not null)
                    ? Results.Ok(readNotificationResponse)
                    : Results.BadRequest("Failed to mark notification as read.");

            }).RequireAuthorization();

            return group;
        }

        public static RouteGroupBuilder MapGetUserNotifications(this RouteGroupBuilder group)
        {
            group.MapPost("/getCurrentNotifications", async (
                IMediator mediator) =>
            {
                var userNotifications = await mediator.Send(new GetCurrentLoggedUserNotificationsQuery());

                return (userNotifications is not null)
                    ? Results.Ok(userNotifications)
                    : Results.BadRequest("Failed to fetch user notifications.");

            }).RequireAuthorization();

            return group;
        }
    }
}
