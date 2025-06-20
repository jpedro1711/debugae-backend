using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using GestaoDefeitos.Domain.Enums;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.ManageContributors
{
    public class ManageContributorHandler
        (
            UserManager<Contributor> userManager,
            IProjectRepository projectRepository,
            IProjectContributorRepository projectContributorRepository
        ) : IRequestHandler<ManageContributorCommand, ManageContributorResponse?>
    {
        public async Task<ManageContributorResponse?> Handle(ManageContributorCommand request, CancellationToken cancellationToken)
        {
            var project = await projectRepository.GetByIdAsync(request.ProjectId)
                                ?? throw new InvalidOperationException("Project not found. Please check the project ID.");

            var contributor = await userManager.FindByEmailAsync(request.ContributorEmail)
                                ?? throw new InvalidOperationException("Contributor not found. Please check the contributor e-mail.");

            var existingProjectContributor = await projectContributorRepository.GetByProjectAndUserIds(project.Id, contributor.Id);

            if (request.IsAdding)
            {
                if (existingProjectContributor is null)
                {
                    var projectContributor = new ProjectContributor
                    {
                        ProjectId = project.Id,
                        ContributorId = contributor.Id,
                        Role = ProjectRole.Contributor,
                    };
                    await projectContributorRepository.AddAsync(projectContributor);
                }
            }
            else
            {
                if (existingProjectContributor is null)
                    throw new InvalidOperationException("Contributor is not part of the project. Please check the contributor e-mail.");

                await projectContributorRepository.RemoveProjectContributor(existingProjectContributor);
            }


            return new ManageContributorResponse(contributor.Id, project.Id, request.IsAdding ? "Added" : "Removed");
        }
    }
}
