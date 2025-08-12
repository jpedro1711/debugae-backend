using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.GetAllProjectFromUser
{
    public record GetAllProjectFromUserResponse(List<UsersProjectViewModel> Data);
}
