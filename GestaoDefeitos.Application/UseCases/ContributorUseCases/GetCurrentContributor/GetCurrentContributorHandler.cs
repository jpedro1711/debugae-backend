using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.ContributorUseCases.GetCurrentContributor
{
    public class GetCurrentContributorHandler(AuthenticationContextAcessor httpContextAccessor, IContributorRepository repository) : IRequestHandler<GetCurrentContributorQuery, GetCurrentContributorResponse?>
    {
        public async Task<GetCurrentContributorResponse?> Handle(GetCurrentContributorQuery request, CancellationToken cancellationToken)
        {
            var userId = httpContextAccessor.GetCurrentLoggedUserId();

            var user = await repository.GetByIdAsync(userId);
        
            return new GetCurrentContributorResponse(
                user?.Id ?? Guid.Empty,
                user?.Firstname ?? string.Empty,
                user?.Lastname ?? string.Empty,
                user?.Email ?? string.Empty,
                user?.Department ?? string.Empty,
                user?.ContributorProfession.ToString() ?? string.Empty
            );
        }
    }
}
