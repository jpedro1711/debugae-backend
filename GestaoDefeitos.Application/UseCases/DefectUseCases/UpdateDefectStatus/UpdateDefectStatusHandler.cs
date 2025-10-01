using GestaoDefeitos.Application.UseCases.DefectUseCases.DefectChangedEvent;
using GestaoDefeitos.Application.UseCases.DefectUseCases.NotifyDefectMailLetter;
using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.UpdateDefectStatus
{
    public class UpdateDefectStatusHandler
        (
            IDefectRepository defectRepository,
            AuthenticationContextAcessor authenticationContextAcessor,
            IContributorNotificationRepository contributorNotificationRepository,
            IMediator mediator
        ) : IRequestHandler<UpdateDefectStatusCommand, UpdateDefectStatusResponse?>
    {
        public async Task<UpdateDefectStatusResponse?> Handle(UpdateDefectStatusCommand request, CancellationToken cancellationToken)
        {
            var defect = await defectRepository.GetByIdAsync(request.DefectId);

            if (defect == null)
                return null;

            var loggedUserId = authenticationContextAcessor.GetCurrentLoggedUserId();

            await mediator.Publish(new DefectChangeNotification
            {
                DefectId = defect.Id,
                ContributorId = loggedUserId,
                Action = DefectAction.Update,
                Field = nameof(Defect.Status),
                OldValue = defect.Status.ToString(),
                NewValue = request.NewStatus.ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }, cancellationToken);

            defect.Status = request.NewStatus;

            var updatedDefect = await defectRepository.UpdateAsync(defect);

            if (updatedDefect is null)
                return null;

            if (loggedUserId != defect.AssignedToContributorId)
                await SaveUserNotification(defect.AssignedToContributorId, contributorNotificationRepository, updatedDefect.Id);

            await NotificateEmailLetter(updatedDefect.Id, $"O defeito {updatedDefect.Id} mudou de status.", mediator);

            return new UpdateDefectStatusResponse(defect.Id, defect.Status);
        }

        private static async Task SaveUserNotification(Guid userId, IContributorNotificationRepository notificationRepository, Guid defectId)
        {
            var notification = new ContributorNotification
            {
                Id = Guid.NewGuid(),
                ContributorId = userId,
                Content = $"Um defeito mudou de status - {defectId}",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await notificationRepository.AddAsync(notification);
        }

        public static async Task NotificateEmailLetter(Guid defectId, string content, IMediator mediator)
        {
            var notification = new NotifyDefectMailLetterNotification
            {
                DefectId = defectId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            await mediator.Publish(notification);
        }
    }
}
