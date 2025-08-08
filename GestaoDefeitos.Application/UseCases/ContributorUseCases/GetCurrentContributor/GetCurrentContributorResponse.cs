namespace GestaoDefeitos.Application.UseCases.ContributorUseCases.GetCurrentContributor
{
    public record GetCurrentContributorResponse(
        Guid ContributorId, 
        string FirstName, 
        string LastName, 
        string Email,
        string Department,
        string Position
    );
}
