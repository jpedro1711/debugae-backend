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

            userSubscribedToLetterIds.ForEach(async (user) =>
            {
                if (loggedUserId != user)
                {
                    var userNotification = new ContributorNotification
                    {
                        Id = Guid.NewGuid(),
                        ContributorId = user,
                        Content = notification.Content,
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow
                    };

                    await contributorNotificationRepository.AddAsync(userNotification);
                }
            });
        }
    }
}
