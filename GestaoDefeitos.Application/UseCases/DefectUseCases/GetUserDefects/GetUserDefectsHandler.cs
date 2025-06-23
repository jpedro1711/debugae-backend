using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetUserDefects
{
    public class GetUserDefectsHandler(
        IDefectRepository defectRepository, 
        AuthenticationContextAcessor authenticationContextAcessor
        ) : IRequestHandler<GetUserDefectsQuery, GetUserDefectsResponse>
    {
        public async Task<GetUserDefectsResponse> Handle(GetUserDefectsQuery request, CancellationToken cancellationToken)
        {
            var loggedUserId = authenticationContextAcessor.GetCurrentLoggedUserId();
            var userDefects = await defectRepository.GetDefectsByContributorPagedAsync(loggedUserId, request.Page, request.PageSize, cancellationToken);

            return new GetUserDefectsResponse(userDefects);
        }
    }
}
