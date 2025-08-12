using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.GetAllProjectFromUser
{
    public class GetAllProjectsFromUserHandler(IProjectContributorRepository projectRepository, AuthenticationContextAcessor authenticationContextAcessor)
        : IRequestHandler<GetAllProjectsFromUserQuery, GetAllProjectFromUserResponse>
    {
        public async Task<GetAllProjectFromUserResponse> Handle(GetAllProjectsFromUserQuery request, CancellationToken cancellationToken)
        {
            var currentLoggedUserId = authenticationContextAcessor.GetCurrentLoggedUserId();
            var projects = await projectRepository.GetAllProjectContributorsByUserIdAsync(currentLoggedUserId);
            var response = new GetAllProjectFromUserResponse(projects);
            return response;
        }
    }
}
