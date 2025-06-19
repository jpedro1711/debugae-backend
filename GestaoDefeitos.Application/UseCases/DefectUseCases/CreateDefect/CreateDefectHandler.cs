using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Security.Claims;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.CreateDefect
{
    public class CreateDefectHandler(
        IDefectRepository defectRepository,
        IDefectAttachmentRepository defectAttachmentRepository,
        IDefectHistoryRepository defectHistoryRepository,
        IHttpContextAccessor httpContextAccessor,
        IDefectRelationRepository defectRelationRepository,
        IProjectContributorRepository projectContributorRepository,
        IContributorNotificationRepository contributorNotificationRepository,
        IProjectRepository projectRepository
        ) : IRequestHandler<CreateDefectCommand, CreateDefectResponse?>
    {
        public async Task<CreateDefectResponse?> Handle(CreateDefectCommand command, CancellationToken cancellationToken)
        {
            var newDefect = MapCommandToNewDefect(command);

            newDefect.ExpiresIn = GetDefectExpirationDate(newDefect);

            var loggedUserId = Guid.Parse(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            await ValidateProjectAndUser(command, projectContributorRepository, projectRepository);

            var savedDefect = await defectRepository.AddAsync(newDefect);

            if (command.Attachment is not null)
                await SaveDefectAttachment(command.Attachment, defectAttachmentRepository, newDefect);

            // Someone if assigned the defect to other user, so we have to notify the assigned user
            if (new Guid(command.AssignedToUserId) != loggedUserId)
                await SaveUserNotification(new Guid(command.AssignedToUserId), contributorNotificationRepository, newDefect.Id);

            await CreateDefectHistory(newDefect, defectHistoryRepository, loggedUserId);

            await RelateToDuplicates(command, defectRepository, savedDefect.Id);

            return new CreateDefectResponse(savedDefect.Id.ToString());
        }

        private static async Task ValidateProjectAndUser(CreateDefectCommand command, IProjectContributorRepository projectContributorRepository, IProjectRepository projectRepository)
        {
            var projectExists = await projectRepository.GetByIdAsync(new Guid(command.ProjectId)) is not null;

            if (!projectExists)
                throw new InvalidOperationException("The project does not exist.");

            var isAssignedUserInProject = await projectContributorRepository.IsUserOnProject(new Guid(command.AssignedToUserId), new Guid(command.ProjectId));

            if (!isAssignedUserInProject)
                throw new InvalidOperationException("The user assigned to the defect is not part of the project.");
        }

        private static async Task SaveUserNotification(Guid userId, IContributorNotificationRepository notificationRepository, Guid defectId)
        {
            var notification = new ContributorNotification
            {
                Id = Guid.NewGuid(),
                ContributorId = userId,
                Content = $"You have a new defect assigned to you - {defectId}",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await notificationRepository.AddAsync(notification);
        }

        private async Task RelateToDuplicates(CreateDefectCommand command, IDefectRepository defectRepository, Guid savedDefectId)
        {
            if (command.DuplicatesIds is null) return;

            var duplicatesGuids = JsonConvert.DeserializeObject<List<Guid>>(command.DuplicatesIds[0])!;

            foreach (var duplicateId in duplicatesGuids)
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
                AssignedToContributorId = new Guid(command.AssignedToUserId),
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

        private static async Task<DefectAttachment> SaveDefectAttachment(IFormFile attachment, IDefectAttachmentRepository repository, Defect newDefect)
        {
            using var memoryStream = new MemoryStream();
            await attachment.CopyToAsync(memoryStream);

            var Attachment = new DefectAttachment
            {
                FileName = attachment.FileName,
                FileType = attachment.ContentType,
                FileContent = memoryStream.ToArray(),
                DefectId = newDefect.Id
            };

            var savedAttachment = await repository.AddAsync(Attachment);

            newDefect.Attachment = savedAttachment;

            return savedAttachment;
        }

        private static async Task<DefectHistory> CreateDefectHistory(
            Defect defect, 
            IDefectHistoryRepository repository,
            Guid contributorId
           )
        {
            var history = new DefectHistory
            {
                Id = Guid.NewGuid(),
                DefectId = defect.Id,
                ContributorId = contributorId,
                Action = DefectAction.Create,
                OldMetadataJson = string.Empty,
                NewMetadataJson = JsonConvert.SerializeObject(defect, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
            };

            return await repository.AddAsync(history);
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
