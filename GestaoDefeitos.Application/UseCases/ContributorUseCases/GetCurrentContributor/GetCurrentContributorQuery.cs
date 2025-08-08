using MediatR;

namespace GestaoDefeitos.Application.UseCases.ContributorUseCases.GetCurrentContributor
{
    public record GetCurrentContributorQuery() : IRequest<GetCurrentContributorResponse?>;
}
