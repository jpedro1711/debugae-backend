namespace GestaoDefeitos.Domain.ViewModels
{
    public record UsersProjectViewModel(
        string ProjectId,
        string ProjectName,
        string ProjectDescription,
        int MembersCount,
        string UserProjectRole,
        List<ProjectColaboratorViewModel> Colaborators
        )
    {
    }
}
