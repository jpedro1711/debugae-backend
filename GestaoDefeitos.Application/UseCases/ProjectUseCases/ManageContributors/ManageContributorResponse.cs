namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.ManageContributors
{
    public record ManageContributorResponse(Guid ContributorId, Guid ProjectId, string Action);
}
