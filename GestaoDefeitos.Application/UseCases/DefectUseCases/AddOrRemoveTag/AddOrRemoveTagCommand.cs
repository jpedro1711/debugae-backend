using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.AddOrRemoveTag
{
    public record AddOrRemoveTagCommand(Guid DefectId, string TagValue) : IRequest<AddOrRemoveTagResponse>;
}
