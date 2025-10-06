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

            (int totalOpen, int totalClosed, int totalResolved, int totalInvalid, int totalReopened, int totalWaitingForUser) = GetDefectCountByStatus(projectDetails.Defects);

            var response = CreateProjectDetailsResponse(
                projectDetails,
                totalOpen,
                totalClosed,
                totalResolved,
                totalInvalid,
                totalReopened,
                totalWaitingForUser
            );

            return response;
        }

        private static GetProjectDetailsResponse CreateProjectDetailsResponse(
            Project project,
            int totalOpen,
            int totalClosed,
            int totalResolved,
            int totalInvalid,
            int totalReopened,
            int totalWaitingForUser
            )
        {
            return new GetProjectDetailsResponse(
                    project.Id,
                    project.Name,
                    project.Description,
                    project.CreatedAt,
                    project.ProjectContributors.Count,
                    project.Defects.Count,
                    totalOpen,
                    totalClosed,
                    totalResolved,
                    totalInvalid,
                    totalReopened,
                    totalWaitingForUser,
                    project.ProjectContributors.Select(pc => new ProjectColaboratorViewModel(
                        pc.Contributor.Id,
                        pc.Contributor.Firstname + " " + pc.Contributor.Lastname,
                        pc.Contributor.ProjectContributors.Single(x => x.ContributorId == pc.ContributorId).Role.ToString(),
                        pc.Contributor.Email!
                    )).ToList(),
                    project.Defects.Select(d => new DefectsSimplifiedViewModel(
                        d.Id,
                        d.Description,
                        d.Summary,
                        d.Status.ToString(),
                        d.DefectPriority.ToString(),
                        d.ExpiresIn,
                        d.CreatedAt,
                        d.DefectCategory.ToString(),
                        new ProjectSimplifiedViewModel(
                            project.Id,
                            project.Name,
                            project.Description,
                            project.CreatedAt
                        ),
                        d.Tags.Select(t => t.Description).ToList(),
                        d.Version,
                        d.DefectHistory.FirstOrDefault(e => e.NewValue == DefectStatus.Resolved.ToString())?.CreatedAt
                    )).ToList()
                );
        }

        private static (int totalOpen, int totalClosed, int totalResolved, int totalInvalid, int totalReopened, int totalWaitingForUser) GetDefectCountByStatus(List<Defect> defects)
        {
            var totalNew = defects.Count(d => d.Status == DefectStatus.New);
            var totalInProgress = defects.Count(d => d.Status == DefectStatus.InProgress);
            var totalResolved = defects.Count(d => d.Status == DefectStatus.Resolved);
            var totalInvalid = defects.Count(d => d.Status == DefectStatus.Invalid);
            var totalReopened = defects.Count(d => d.Status == DefectStatus.Reopened);
            var totalWaitingForUser = defects.Count(d => d.Status == DefectStatus.Reopened);

            return (totalNew, totalInProgress, totalResolved, totalInvalid, totalReopened, totalWaitingForUser);
        }
    }
}
