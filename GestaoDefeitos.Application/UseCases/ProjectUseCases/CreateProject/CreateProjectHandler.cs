using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.CreateProject
{
    public class CreateProjectHandler(
        IProjectRepository projectRepository,
        IContributorRepository contributorRepository,
        IProjectContributorRepository projectContributorRepository,
        AuthenticationContextAcessor authenticationContextAcessor
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

            var loggedUserId = authenticationContextAcessor.GetCurrentLoggedUserId();

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
