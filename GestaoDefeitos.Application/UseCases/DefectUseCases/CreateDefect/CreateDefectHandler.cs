using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.CreateDefect
{
    public class CreateDefectHandler(
        IDefectRepository defectRepository,
        IDefectAttachmentRepository defectAttachmentRepository,
        IDefectHistoryRepository defectHistoryRepository,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<CreateDefectCommand, CreateDefectResponse?>
    {
        public async Task<CreateDefectResponse?> Handle(CreateDefectCommand command, CancellationToken cancellationToken)
        {
            var newDefect = MapCommandToNewDefect(command);

            if (command.Attachment is not null)
            {
                newDefect.Attachment = await SaveDefectAttachment(command.Attachment, defectAttachmentRepository);
            }

            newDefect.ExpiresIn = GetDefectExpirationDate(newDefect);

            var loggedUserId = Guid.Parse(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var savedCretionHistory = await CreateDefectHistory(newDefect, defectHistoryRepository, loggedUserId);

            newDefect.History.Add(savedCretionHistory);

            var savedDefect = await defectRepository.AddAsync(newDefect);

            return new CreateDefectResponse(savedDefect.Id.ToString());
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

        private static async Task<DefectAttachment> SaveDefectAttachment(IFormFile attachment, IDefectAttachmentRepository repository)
        {
            using var memoryStream = new MemoryStream();
            await attachment.CopyToAsync(memoryStream);

            var Attachment = new DefectAttachment
            {
                FileName = attachment.FileName,
                FileType = attachment.ContentType,
                FileContent = memoryStream.ToArray(),
            };

            var savedAttachment = await repository.AddAsync(Attachment);

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
                ContributorId = contributorId,
                Action = DefectAction.Create,
                OldMetadataJson = string.Empty,
                NewMetadataJson = JsonSerializer.Serialize(defect),
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
