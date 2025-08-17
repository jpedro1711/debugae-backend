using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetAllDefectsFromUser
{
    public class GetAllDefectsFromUserHandler(
        IDefectRepository defectRepository,
        AuthenticationContextAcessor authenticationContextAcessor) : IRequestHandler<GetAllDefectsFromUserQuery, GetAllDefectsFromUserResponse>
    {
        public async Task<GetAllDefectsFromUserResponse> Handle(GetAllDefectsFromUserQuery query, CancellationToken cancellationToken)
        {
            var loggedUserId = authenticationContextAcessor.GetCurrentLoggedUserId();
            var userDefects = await defectRepository.GetDefectsByContributorAsync(loggedUserId, cancellationToken);

            return new GetAllDefectsFromUserResponse(userDefects);
        }
    }
}
