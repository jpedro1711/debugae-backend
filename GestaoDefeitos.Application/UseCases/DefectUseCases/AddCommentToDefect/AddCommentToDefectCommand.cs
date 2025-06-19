using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.AddCommentToDefect
{
    public record AddCommentToDefectCommand(Guid DefectId, string Comment) : IRequest<AddCommentToDefectResponse?>;
}
