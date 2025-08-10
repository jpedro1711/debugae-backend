using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetDefectDetails
{
    public class GetDefectDetailsHandler
        (
            IDefectRepository defectRepository,
            IDefectHistoryRepository defectHistoryRepository
        ) : IRequestHandler<GetDefectDetailsQuery, GetDefectDetailsResponse?>
    {
        public async Task<GetDefectDetailsResponse?> Handle(GetDefectDetailsQuery query, CancellationToken cancellationToken)
        {
            var defectHistory = await defectHistoryRepository.GetDefectHistoryByDefectIdAsync(query.DefectId, cancellationToken);

            var defectFullDetails = await defectRepository.GetDefectDetails(query.DefectId, cancellationToken);

            var response = defectFullDetails with { History = defectHistory };

            return new GetDefectDetailsResponse(response); 
        }
    }
}
