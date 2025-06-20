using MediatR;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.ManageContributors
{
    public record ManageContributorCommand(Guid ProjectId, string ContributorEmail, bool IsAdding) : IRequest<ManageContributorResponse?>;
}
