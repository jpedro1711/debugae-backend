using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.AddOrRemoveCurrentUserToMailLetter
{
    public class AddCurrentUserToMailLetterHandler(
            AuthenticationContextAcessor authenticationContextAcessor,
            IDefectMailLetterRepository defectMailLetterRepository
        ) 
        : IRequestHandler<AddCurrentUserToMailLetterRequest, AddCurrentUserToMailLetterResponse?>
    {
        public async Task<AddCurrentUserToMailLetterResponse?> Handle(
            AddCurrentUserToMailLetterRequest request, 
            CancellationToken cancellationToken
            )
        {
            var currentUserId = authenticationContextAcessor.GetCurrentLoggedUserId();
            var defectMailLetter = await defectMailLetterRepository.GetByCompositeIdAsync(request.DefectId, currentUserId);
            
            if (defectMailLetter is not null)
            {
                await defectMailLetterRepository.RemoveFromMailLetter(request.DefectId, currentUserId);
            }
            else
            {
                await defectMailLetterRepository.AddToMailLetter(request.DefectId, currentUserId);
            }

            return new AddCurrentUserToMailLetterResponse(currentUserId, request.DefectId);
        }
    }
}
