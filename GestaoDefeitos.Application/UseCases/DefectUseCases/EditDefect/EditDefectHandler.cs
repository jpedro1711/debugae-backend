using GestaoDefeitos.Application.UseCases.DefectUseCases.DefectChangedEvent;
using GestaoDefeitos.Application.UseCases.DefectUseCases.NotifyDefectMailLetter;
using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.EditDefect
{
    public class EditDefectHandler(
    IDefectRepository defectRepository,
    AuthenticationContextAcessor authenticationContextAcessor,
    IContributorRepository contributorRepository,
    IMediator mediator
) : IRequestHandler<EditDefectCommand, EditDefectResponse>
    {
        public async Task<EditDefectResponse> Handle(EditDefectCommand command, CancellationToken cancellationToken)
        {
            var userId = authenticationContextAcessor.GetCurrentLoggedUserId();
            var defect = await defectRepository.GetByIdAsync(command.DefectId)
                ?? throw new KeyNotFoundException($"Defeito com ID {command.DefectId} não encontrado.");

            var changes = new List<DefectChangeNotification>();

            UpdateField(defect, nameof(defect.Description), defect.Description, command.NewDescription, v => defect.Description = v, userId, changes);
            UpdateField(defect, nameof(defect.DefectEnvironment), defect.DefectEnvironment.ToString(), command.NewEnvironment.ToString(), v => defect.DefectEnvironment = command.NewEnvironment, userId, changes);
            UpdateField(defect, nameof(defect.DefectSeverity), defect.DefectSeverity.ToString(), command.NewSeverity.ToString(), v => defect.DefectSeverity = command.NewSeverity, userId, changes);
            UpdateField(defect, nameof(defect.Status), defect.Status.ToString(), command.NewStatus.ToString(), v => defect.Status = command.NewStatus, userId, changes);
            UpdateField(defect, nameof(defect.DefectCategory), defect.DefectCategory.ToString(), command.NewCategory.ToString(), v => defect.DefectCategory = command.NewCategory, userId, changes);
            UpdateField(defect, nameof(defect.ActualBehaviour), defect.ActualBehaviour, command.NewCurrentBehaviour, v => defect.ActualBehaviour = v, userId, changes);
            UpdateField(defect, nameof(defect.ExpectedBehaviour), defect.ExpectedBehaviour, command.NewExpectedBehaviour, v => defect.ExpectedBehaviour = v, userId, changes);
            UpdateField(defect, nameof(defect.ErrorLog), defect.ErrorLog, command.NewStackTrace, v => defect.ErrorLog = v, userId, changes);

            var assignedContributor = await contributorRepository.GetContributorByEmailAsync(command.NewAssignedToContributorEmail)
                ?? throw new KeyNotFoundException($"Contribuidor com email {command.NewAssignedToContributorEmail} não encontrado.");

            UpdateField(defect, nameof(defect.AssignedToContributorId), defect.AssignedToContributorId.ToString(), assignedContributor.Id.ToString(), v => defect.AssignedToContributorId = assignedContributor.Id, userId, changes);

            var updatedDefect = await defectRepository.UpdateAsync(defect)
                ?? throw new InvalidOperationException("Erro ao atualizar o defeito.");

            foreach (var change in changes)
            {
                await mediator.Publish(change, cancellationToken);

                if (change.Field == nameof(Defect.AssignedToContributorId) && userId == updatedDefect.AssignedToContributorId)
                {
                    await mediator.Publish(new NotifyDefectMailLetterNotification { DefectId = updatedDefect.Id, Content = $"O defeito {updatedDefect.Id} foi assinalado ao seu usuário" }, cancellationToken);
                }
                else
                {
                    await mediator.Publish(new NotifyDefectMailLetterNotification { DefectId = updatedDefect.Id, Content = $"O defeito {updatedDefect.Id} foi atualizado - {change.Field}" }, cancellationToken);
                }
            }

            return new EditDefectResponse(updatedDefect.Id.ToString());
        }

        private static void UpdateField(
            object entity,
            string fieldName,
            string oldValue,
            string newValue,
            Action<string> setValue,
            Guid userId,
            List<DefectChangeNotification> changes)
        {
            if (oldValue != newValue)
            {
                setValue(newValue);
                changes.Add(new DefectChangeNotification
                {
                    DefectId = ((Defect)entity).Id,
                    ContributorId = userId,
                    Action = DefectAction.Update,
                    Field = fieldName,
                    OldValue = oldValue,
                    NewValue = newValue,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }
    }

}
