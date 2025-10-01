using GestaoDefeitos.Application.UseCases.DefectUseCases.NotifyDefectMailLetter;
using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.AddCommentToDefect
{
    public class AddCommentToDefectHandler
        (
            IDefectRepository defectRepository,
            IDefectCommentRepository defectCommentRepository,
            AuthenticationContextAcessor authenticationContextAcessor,
            IMediator mediator
        ) : IRequestHandler<AddCommentToDefectCommand, AddCommentToDefectResponse?>
    {
        public async Task<AddCommentToDefectResponse?> Handle(AddCommentToDefectCommand command, CancellationToken cancellationToken)
        {
            var defect = await defectRepository.GetByIdAsync(command.DefectId);

            if (defect is null)
                return null;

            var loggedUserId = authenticationContextAcessor.GetCurrentLoggedUserId();

            var newComment = new DefectComment
            {
                Id = Guid.NewGuid(),
                Content = command.Comment,
                ContributorId = loggedUserId,
                CreatedAt = DateTime.UtcNow,
                DefectId = defect.Id
            };

            var createdComment = await defectCommentRepository.AddAsync(newComment);

            await mediator.Publish(new NotifyDefectMailLetterNotification { DefectId = defect.Id, Content = $"Comentários foram adicionados ao defeito ${defect.Id}" }, cancellationToken);

            return new AddCommentToDefectResponse(createdComment.DefectId, createdComment.Id);
        }
    }
}
