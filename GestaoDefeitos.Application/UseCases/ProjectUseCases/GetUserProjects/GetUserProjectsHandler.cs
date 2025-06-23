using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.GetUserProjects
{
    public class GetUserProjectsHandler(
            IProjectContributorRepository projectContributorRepository,
            AuthenticationContextAcessor authenticationContextAcessor
        ) : IRequestHandler<GetUserProjectsQuery, GetUserProjectsResponse>
    {
        public async Task<GetUserProjectsResponse> Handle(GetUserProjectsQuery query, CancellationToken cancellationToken)
        {
            var loggedUserId = authenticationContextAcessor.GetCurrentLoggedUserId();

            return new GetUserProjectsResponse(await projectContributorRepository.GetProjectContributorsByUserIdAsync(loggedUserId, query.Page, query.PageSize));
        }
    }
}
