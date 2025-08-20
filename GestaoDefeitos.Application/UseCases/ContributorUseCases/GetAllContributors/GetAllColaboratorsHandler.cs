using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.ContributorUseCases.GetAllContributors
{
    public class GetAllColaboratorsHandler(IContributorRepository contributorRepository) : IRequestHandler<GetAllContributorsQuery, GetAllContributorsResponse>
    {
        public async Task<GetAllContributorsResponse> Handle(GetAllContributorsQuery query, CancellationToken cancellationToken)
        {
            var result = await contributorRepository.GetAllColaborators();

            return new GetAllContributorsResponse(result);
        }
    }
}
