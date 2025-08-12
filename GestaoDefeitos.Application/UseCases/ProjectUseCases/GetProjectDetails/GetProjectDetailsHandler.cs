using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Domain.ViewModels;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.GetProjectDetails
{
    public class GetProjectDetailsHandler
        (
            IProjectRepository projectRepository
        ) : IRequestHandler<GetProjectDetailsQuery, GetProjectDetailsResponse?>
    {
        public async Task<GetProjectDetailsResponse?> Handle(GetProjectDetailsQuery request, CancellationToken cancellationToken)
        {
            var projectDetails = await projectRepository.GetProjectDetailsAsync(request.ProjectId, cancellationToken);

            if (projectDetails is null)
                return null;

            (int totalOpen, int totalInProgress, int totalResolved) = GetDefectCountByStatus(projectDetails.Defects);

            var response = CreateProjectDetailsResponse(
                projectDetails,
                totalOpen,
                totalInProgress,
                totalResolved
            );

            return response;
        }

        private static GetProjectDetailsResponse CreateProjectDetailsResponse(
            Project project, 
            int totalDefectsOpen, 
            int totalDefectsInProgress, 
            int totalDefectsResolved
            )
        {
            return new GetProjectDetailsResponse(
                    project.Id,
                    project.Name,
                    project.Description,
                    project.CreatedAt,
                    project.ProjectContributors.Count,
                    project.Defects.Count,
                    totalDefectsOpen,
                    totalDefectsInProgress,
                    totalDefectsResolved,
                    project.ProjectContributors.Select(pc => new ProjectColaboratorViewModel(
                        pc.Contributor.Id,
                        pc.Contributor.Firstname + " " + pc.Contributor.Lastname,
                        pc.Contributor.ProjectContributors.Single(x => x.ContributorId == pc.ContributorId).Role.ToString()
                    )).ToList(),
                    project.Defects.Select(d => new DefectsSimplifiedViewModel(
                        d.Id,
                        d.Description,
                        d.Summary,
                        d.Status.ToString(),
                        d.DefectPriority.ToString(),
                        d.ExpiresIn,
                        d.CreatedAt
                    )).ToList()
                );
        }

        private static (int totalOpen, int totalClosed, int totalResolved) GetDefectCountByStatus(List<Defect> defects)
        {
            var totalOpen = defects.Count(d => d.Status == DefectStatus.New);
            var totalClosed = defects.Count(d => d.Status == DefectStatus.InProgress);
            var totalResolved = defects.Count(d => d.Status == DefectStatus.Resolved);

            return (totalOpen, totalClosed, totalResolved);
        }
    }
}
