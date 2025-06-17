namespace GestaoDefeitos.Domain.ViewModels
{
    public record UsersProjectViewModel(string projectId, string ProjectName, string ProjectDescription, int MembersCount, string UserProjectRole)
    {
    }
}
