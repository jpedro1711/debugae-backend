using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetUserDefects
{
    public class GetUserDefectsHandler(
        IDefectRepository defectRepository, 
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<GetUserDefectsQuery, GetUserDefectsResponse>
    {
        public async Task<GetUserDefectsResponse> Handle(GetUserDefectsQuery request, CancellationToken cancellationToken)
        {
            var loggedUserId = Guid.Parse(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var userDefects = await defectRepository.GetDefectsByContributorAsync(loggedUserId, cancellationToken);

            return new GetUserDefectsResponse(userDefects);
        }
    }
}
