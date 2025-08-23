using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.GetProjectDetails
{
    public record GetProjectDetailsResponse(
            Guid ProjectId,
            string ProjectName,
            string ProjectDescription,
            DateTime CreatedAt,
            int TotalColaborators,
            int TotalDefects,
            int TotalOpen,
            int TotalClosed,
            int TotalResolved,
            int TotalInvalid,
            int TotalReopened,
            int TotalWaitingForUser,
            List<ProjectColaboratorViewModel> Colaborators,
            List<DefectsSimplifiedViewModel> Defects
        );
}
