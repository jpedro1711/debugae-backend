using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.GetUserProjects
{
    public record GetUserProjectsResponse(List<UsersProjectViewModel> UserProjects);
}
