using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.AddOrRemoveCurrentUserToMailLetter
{
    public record AddCurrentUserToMailLetterRequest(Guid DefectId) : IRequest<AddCurrentUserToMailLetterResponse?>;
}
