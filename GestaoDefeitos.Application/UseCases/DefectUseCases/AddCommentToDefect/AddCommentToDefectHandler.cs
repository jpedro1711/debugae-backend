using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.AddCommentToDefect
{
    public class AddCommentToDefectHandler
        (
            IDefectRepository defectRepository,
            IDefectCommentRepository defectCommentRepository,
            IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<AddCommentToDefectCommand, AddCommentToDefectResponse?> 
    {
        public async Task<AddCommentToDefectResponse?> Handle(AddCommentToDefectCommand command, CancellationToken cancellationToken)
        {
            var defect = await defectRepository.GetByIdAsync(command.DefectId);

            if (defect is null)
                return null;

            var loggedUserId = Guid.Parse(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var newComment = new DefectComment
            {
                Id = Guid.NewGuid(),
                Content = command.Comment,
                ContributorId = loggedUserId,
                CreatedAt = DateTime.UtcNow,
                DefectId = defect.Id
            };

            var createdComment = await defectCommentRepository.AddAsync(newComment);

            return new AddCommentToDefectResponse(createdComment.DefectId, createdComment.Id);
        }
    }
}
