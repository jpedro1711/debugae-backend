using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.NotifyDefectMailLetter
{
    public class NotifyDefectMailLetterEventHandler(IDefectMailLetterRepository defectMailLetterRepository, IContributorNotificationRepository contributorNotificationRepository, AuthenticationContextAcessor authenticationContext) : INotificationHandler<NotifyDefectMailLetterNotification>
    {
        public async Task Handle(NotifyDefectMailLetterNotification notification, CancellationToken cancellationToken)
        {
            var userSubscribedToLetterIds = await defectMailLetterRepository.GetEnrolledContributorsIds(notification.DefectId);
            var loggedUserId = authenticationContext.GetCurrentLoggedUserId();

            var notifications = userSubscribedToLetterIds
                .Where(user => user != loggedUserId)
                .Select(user => new ContributorNotification
                {
                    Id = Guid.NewGuid(),
                    ContributorId = user,
                    Content = notification.Content,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                })
                .ToList();

            if (notifications.Count > 0)
            {
                await contributorNotificationRepository.AddRangeAsync(notifications);
            }
        }

    }
}
