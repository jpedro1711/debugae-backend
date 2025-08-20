using MediatR;

namespace GestaoDefeitos.Application.UseCases.ContributorUseCases.GetAllContributors
{
    public record GetAllContributorsQuery() : IRequest<GetAllContributorsResponse>;
}
