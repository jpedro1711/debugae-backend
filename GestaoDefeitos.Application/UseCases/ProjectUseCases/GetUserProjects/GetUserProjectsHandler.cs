using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.GetUserProjects
{
    public class GetUserProjectsHandler(
            IProjectContributorRepository projectContributorRepository,
            IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<GetUserProjectsQuery, GetUserProjectsResponse>
    {
        public async Task<GetUserProjectsResponse> Handle(GetUserProjectsQuery query, CancellationToken cancellationToken)
        {
            var loggedUserId = Guid.Parse(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            return new GetUserProjectsResponse(await projectContributorRepository.GetProjectContributorsByUserIdAsync(loggedUserId));
        }
    }
}
