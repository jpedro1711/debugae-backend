using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;
using Newtonsoft.Json;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.UpdateDefectStatus
{
    public class UpdateDefectStatusHandler
        (
            IDefectRepository defectRepository, 
            IDefectHistoryRepository defectHistoryRepository,
            AuthenticationContextAcessor authenticationContextAcessor,
            IContributorNotificationRepository contributorNotificationRepository
        ) : IRequestHandler<UpdateDefectStatusCommand, UpdateDefectStatusResponse?>
    {
        public async Task<UpdateDefectStatusResponse?> Handle(UpdateDefectStatusCommand request, CancellationToken cancellationToken)
        {
            var defect = await defectRepository.GetByIdAsync(request.DefectId);

            if (defect == null)
                return null;

            var loggedUserId = authenticationContextAcessor.GetCurrentLoggedUserId();

            var defectHistory = CreateDefectHistory(defect, loggedUserId);

            defect.Status = request.NewStatus;

            var updatedDefect = await defectRepository.UpdateAsync(defect);

            if (updatedDefect is null)
                return null;

            defectHistory.NewMetadataJson = Serializer.Serialize(updatedDefect);
            await defectHistoryRepository.AddAsync(defectHistory);

            if (loggedUserId != defect.AssignedToContributorId)
                await SaveUserNotification(defect.AssignedToContributorId, contributorNotificationRepository, updatedDefect.Id);

            return new UpdateDefectStatusResponse(defect.Id, defect.Status);
        }

        private static DefectHistory CreateDefectHistory(Defect defect, Guid loggedUserId)
        {
            return new DefectHistory
            {
                Id = Guid.NewGuid(),
                DefectId = defect.Id,
                ContributorId = loggedUserId,
                Action = DefectAction.Update,
                OldMetadataJson = Serializer.Serialize(defect),
                CreatedAt = defect.CreatedAt,
                UpdatedAt = DateTime.UtcNow,
            };
        }

        private static async Task SaveUserNotification(Guid userId, IContributorNotificationRepository notificationRepository, Guid defectId)
        {
            var notification = new ContributorNotification
            {
                Id = Guid.NewGuid(),
                ContributorId = userId,
                Content = $"Some of your defects had a status change - {defectId}",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await notificationRepository.AddAsync(notification);
        }
    }
}
