using GestaoDefeitos.Application.UseCases.DefectUseCases.DefectChangedEvent;
using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.CreateDefect
{
    public class CreateDefectHandler(
        IDefectRepository defectRepository,
        IDefectAttachmentRepository defectAttachmentRepository,
        AuthenticationContextAcessor authenticationContextAcessor,
        IDefectRelationRepository defectRelationRepository,
        IProjectContributorRepository projectContributorRepository,
        IContributorNotificationRepository contributorNotificationRepository,
        IProjectRepository projectRepository,
        IContributorRepository contributorRepository,
        IMediator mediator
        ) : IRequestHandler<CreateDefectCommand, CreateDefectResponse?>
    {
        public async Task<CreateDefectResponse?> Handle(CreateDefectCommand command, CancellationToken cancellationToken)
        {
            var newDefect = MapCommandToNewDefect(command);

            newDefect.ExpiresIn = GetDefectExpirationDate(newDefect);

            var assignedUser = await contributorRepository.GetContributorByEmailAsync(command.AssignedToUserEmail) ?? throw new InvalidOperationException("The user assigned to the defect does not exist.");

            newDefect.AssignedToContributorId = assignedUser.Id;

            var loggedUserId = authenticationContextAcessor.GetCurrentLoggedUserId();

            var currentUserName = authenticationContextAcessor.GetCurrentLoggedUserName();

            await ValidateProjectAndUser(command, projectContributorRepository, projectRepository, assignedUser);

            var savedDefect = await defectRepository.AddAsync(newDefect);

            if (command.Attachment is not null)
                await SaveDefectAttachment(command.Attachment, defectAttachmentRepository, newDefect, currentUserName ?? "Unknown user");

            // Someone if assigned the defect to other user, so we have to notify the assigned user
            if (assignedUser.Id != loggedUserId)
                await SaveUserNotification(assignedUser.Id, contributorNotificationRepository, newDefect.Id);

            await mediator.Publish(new DefectChangeNotification
            {
                DefectId = savedDefect.Id,
                ContributorId = loggedUserId,
                Action = DefectAction.Create,
                Field = null,
                OldValue = null,
                NewValue = null,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }, cancellationToken);

            await RelateToDuplicates(command, defectRepository, savedDefect.Id);

            return new CreateDefectResponse(savedDefect.Id.ToString());
        }

        private static async Task ValidateProjectAndUser(CreateDefectCommand command, IProjectContributorRepository projectContributorRepository, IProjectRepository projectRepository, Contributor assignedUser)
        {
            var projectExists = await projectRepository.GetByIdAsync(new Guid(command.ProjectId)) is not null;

            if (!projectExists)
                throw new InvalidOperationException("The project does not exist.");

            var isAssignedUserInProject = await projectContributorRepository.IsUserOnProject(assignedUser.Id, new Guid(command.ProjectId));

            if (!isAssignedUserInProject)
                throw new InvalidOperationException("The user assigned to the defect is not part of the project.");
        }

        private static async Task SaveUserNotification(Guid userId, IContributorNotificationRepository notificationRepository, Guid defectId)
        {
            var notification = new ContributorNotification
            {
                Id = Guid.NewGuid(),
                ContributorId = userId,
                Content = $"Você tem um novo defeito assinalado ao seu usuário - {defectId}",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await notificationRepository.AddAsync(notification);
        }

        private async Task RelateToDuplicates(CreateDefectCommand command, IDefectRepository defectRepository, Guid savedDefectId)
        {
            if (command.DuplicatesIds is null) return;

            foreach (var duplicateId in command.DuplicatesIds)
            {
                var duplicateDefect = await defectRepository.GetByIdAsync(duplicateId);
                if (duplicateDefect != null)
                    await defectRelationRepository.AddAsync(new DefectRelation
                    {
                        DefectId = savedDefectId,
                        RelatedDefectId = duplicateDefect.Id,
                    });
            }
        }

        private static Defect MapCommandToNewDefect(CreateDefectCommand command)
        {
            return new Defect
            {
                Id = Guid.NewGuid(),
                ProjectId = new Guid(command.ProjectId),
                Summary = command.Summary,
                Description = command.Description,
                DefectEnvironment = command.Environment,
                DefectSeverity = command.Severity,
                DefectCategory = command.Category,
                Version = command.Version,
                ExpectedBehaviour = command.ExpectedBehaviour,
                ActualBehaviour = command.ActualBehaviour,
                ErrorLog = command.LogTrace,
                Status = DefectStatus.New,
                DefectPriority = command.Priority,
            };
        }

        private static async Task<DefectAttachment> SaveDefectAttachment(IFormFile attachment, IDefectAttachmentRepository repository, Defect newDefect, string uploadedByUsername)
        {
            using var memoryStream = new MemoryStream();
            await attachment.CopyToAsync(memoryStream);

            var Attachment = new DefectAttachment
            {
                FileName = attachment.FileName,
                FileType = attachment.ContentType,
                FileContent = memoryStream.ToArray(),
                DefectId = newDefect.Id,
                UploadByUsername = uploadedByUsername
            };

            var savedAttachment = await repository.AddAsync(Attachment);

            newDefect.Attachment = savedAttachment;

            return savedAttachment;
        }

        private static DateTime GetDefectExpirationDate(Defect defect)
        {
            var today = DateTime.UtcNow;
            return defect.DefectPriority switch
            {
                DefectPriority.P1 => today.AddDays(3),
                DefectPriority.P2 => today.AddDays(5),
                DefectPriority.P3 => today.AddDays(10),
                DefectPriority.P4 => today.AddDays(15),
                _ => today.AddDays(30)
            };
        }
    }
}
