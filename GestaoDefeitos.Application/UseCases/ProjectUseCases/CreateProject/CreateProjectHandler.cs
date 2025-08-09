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
            var project = new Project
            {
                Name = command.ProjectName,
                Description = command.ProjectDescription,
            };

            var createdProject = await projectRepository.AddAsync(project);

            var loggedUserId = authenticationContextAcessor.GetCurrentLoggedUserId();

            foreach (var contributorEmail in command.ContributorsEmails)
            {
                var contributor = await contributorRepository.GetContributorByEmailAsync(contributorEmail);
                if (contributor != null)
                {
                    var projectContributor = new ProjectContributor
                    {
                        ProjectId = createdProject.Id,
                        ContributorId = contributor.Id,
                        Role = (contributor.Id == loggedUserId) ? ProjectRole.Administrator : ProjectRole.Contributor,
                    };

                    await projectContributorRepository.AddAsync(projectContributor);
                }
                else
                {
                    throw new InvalidOperationException("User not found: " + contributorEmail);
                }
            }

            return new CreateProjectResponse(createdProject.Id);
        }
    }
}
