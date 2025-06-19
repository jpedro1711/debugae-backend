using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Security.Claims;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.UpdateDefectStatus
{
    public class UpdateDefectStatusHandler
        (
            IDefectRepository defectRepository, 
            IDefectHistoryRepository defectHistoryRepository,
            IHttpContextAccessor httpContextAccessor,
            IContributorNotificationRepository contributorNotificationRepository
        ) : IRequestHandler<UpdateDefectStatusCommand, UpdateDefectStatusResponse?>
    {
        public async Task<UpdateDefectStatusResponse?> Handle(UpdateDefectStatusCommand request, CancellationToken cancellationToken)
        {
            var defect = await defectRepository.GetByIdAsync(request.DefectId);

            if (defect == null)
                return null;

            var loggedUserId = Guid.Parse(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var defectHistory = new DefectHistory
            {
                Id = Guid.NewGuid(),
                DefectId = defect.Id,
                ContributorId = loggedUserId,
                Action = DefectAction.Update,
                OldMetadataJson = JsonConvert.SerializeObject(defect, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }),
                CreatedAt = defect.CreatedAt,
                UpdatedAt = DateTime.UtcNow,
            };

            defect.Status = request.NewStatus;

            var updatedDefect = await defectRepository.UpdateAsync(defect);

            if (updatedDefect is not null)
            {
                defectHistory.NewMetadataJson = JsonConvert.SerializeObject(updatedDefect, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                await defectHistoryRepository.AddAsync(defectHistory);

                if (loggedUserId != defect.AssignedToContributorId)
                    await SaveUserNotification(defect.AssignedToContributorId, contributorNotificationRepository, updatedDefect.Id);

                return new UpdateDefectStatusResponse(defect.Id, defect.Status);
            }

            return null;
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
