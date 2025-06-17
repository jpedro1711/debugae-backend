using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.CreateProject
{
    public class CreateProjectHandler(
        IProjectRepository projectRepository,
        IContributorRepository contributorRepository,
        IProjectContributorRepository projectContributorRepository,
        IHttpContextAccessor httpContextAccessor
        )
        : IRequestHandler<CreateProjectCommand, CreateProjectResponse?>
    {
        public async Task<CreateProjectResponse?> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
        {
            var contributors = await contributorRepository.GetContributorsByIdsAsync(command.ContributorsIds);

            if (contributors.Count != command.ContributorsIds.Count)
                return null;

            var project = new Project
            {
                Name = command.ProjectName,
                Description = command.ProjectDescription,
            };

            var createdProject = await projectRepository.AddAsync(project);

            var loggedUserId = Guid.Parse(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            foreach (var contributorId in command.ContributorsIds)
            {
                var contributor = contributors.FirstOrDefault(c => c.Id == new Guid(contributorId));
                if (contributor != null)
                {
                    var projectContributor = new ProjectContributor
                    {
                        ProjectId = createdProject.Id,
                        ContributorId = contributor.Id,
                        Role = (new Guid(contributorId) == loggedUserId) ? ProjectRole.Administrator : ProjectRole.Contributor,
                    };

                    await projectContributorRepository.AddAsync(projectContributor);
                }
            }

            return new CreateProjectResponse(createdProject.Id);
        }
    }
}
