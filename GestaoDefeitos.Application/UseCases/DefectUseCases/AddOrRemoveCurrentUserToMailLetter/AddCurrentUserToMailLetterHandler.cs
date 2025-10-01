using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.AddOrRemoveCurrentUserToMailLetter
{
    public class AddCurrentUserToMailLetterHandler(
            AuthenticationContextAcessor authenticationContextAcessor,
            IDefectRepository defectRepository,
            IContributorRepository contributorRepository,
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
            var defect = await defectRepository.GetByIdAsync(request.DefectId) ?? throw new InvalidOperationException("Defeito não existe.");
            
            if (defect.ContributorMailLetter.Any(c => c.ContributorId == currentUserId))
            {
                await defectMailLetterRepository.RemoveFromMailLetter(defect.Id, currentUserId);
            }
            else
            {
                await defectMailLetterRepository.AddToMailLetter(defect.Id, currentUserId);
            }

            await defectRepository.UpdateAsync(defect);

            return new AddCurrentUserToMailLetterResponse(currentUserId, defect.Id);
        }
    }
}
