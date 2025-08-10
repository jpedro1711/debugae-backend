using GestaoDefeitos.Domain.Entities.Events;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.DefectChangedEvent
{
    public class DefectChangeEventHandler(IDefectHistoryRepository defectHistoryRepository) : INotificationHandler<DefectChangeNotification>
    {
        public async Task Handle(DefectChangeNotification defectChangeNotification, CancellationToken cancellationToken)
        {
            var historyToSave = new DefectChangeEvent
            {
                Id = Guid.NewGuid(),
                DefectId = defectChangeNotification.DefectId,
                ContributorId = defectChangeNotification.ContributorId,
                Action = defectChangeNotification.Action,
                Field = defectChangeNotification.Field,
                OldValue = defectChangeNotification.OldValue,
                NewValue = defectChangeNotification.NewValue,
                UpdatedAt = DateTime.UtcNow,
            };

            await defectHistoryRepository.AddAsync(historyToSave);
        }
    }
}
