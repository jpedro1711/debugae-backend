namespace GestaoDefeitos.Application.UseCases.DefectUseCases.AddOrRemoveCurrentUserToMailLetter
{
    public record AddCurrentUserToMailLetterResponse(Guid ContributorId, Guid DefectId);
}
