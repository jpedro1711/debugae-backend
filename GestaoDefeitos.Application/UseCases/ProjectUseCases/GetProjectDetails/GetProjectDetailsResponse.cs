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
            int TotalDefectsOpen,
            int TotalDefectsInProgress,
            int TotalDefectsResolved,
            List<ProjectColaboratorViewModel> Colaborators,
            List<DefectsSimplifiedViewModel> Defects
        );
}
