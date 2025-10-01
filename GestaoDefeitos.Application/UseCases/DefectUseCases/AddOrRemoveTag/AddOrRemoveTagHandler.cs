using GestaoDefeitos.Application.UseCases.DefectUseCases.NotifyDefectMailLetter;
using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.AddOrRemoveTag
{
    public class AddOrRemoveTagHandler(
            ITagRepository tagRepository,
            IDefectRepository defectRepository,
            IMediator mediator
        ) : IRequestHandler<AddOrRemoveTagCommand, AddOrRemoveTagResponse?>
    {
        public async Task<AddOrRemoveTagResponse?> Handle(AddOrRemoveTagCommand command, CancellationToken cancellationToken)
        {
            var defect = await defectRepository.GetByIdAsync(command.DefectId);

            if (defect is null)
                return null;

            var tagLowerValue = command.TagValue.ToLowerInvariant();

            var existingTag = await tagRepository.GetTagByValueAsync(tagLowerValue, cancellationToken);

            // If the tag already exists, we remove it
            if (existingTag is not null)
            {
                await tagRepository.DeleteAsync(existingTag.Id);
                return new AddOrRemoveTagResponse(defect.Id, Guid.Empty);
            }

            // If the tag does not exist, we create a new one
            var newTag = new Tag
            {
                Id = Guid.NewGuid(),
                DefectId = defect.Id,
                Description = tagLowerValue,
                CreatedAt = DateTime.UtcNow,
            };

            var created = await tagRepository.AddAsync(newTag);

            await mediator.Publish(new NotifyDefectMailLetterNotification { DefectId = defect.Id, Content = $"As tags do defeito {defect.Id} foram atualizadas" }, cancellationToken);

            return new AddOrRemoveTagResponse(defect.Id, created.Id);
        }
    }
}
