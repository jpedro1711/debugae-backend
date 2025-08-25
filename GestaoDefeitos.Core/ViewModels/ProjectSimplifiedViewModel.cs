namespace GestaoDefeitos.Domain.ViewModels
{
    public record ProjectSimplifiedViewModel(Guid ProjectId, string ProjectName, string ProjectDescription, DateTime CreatedAt);
}
