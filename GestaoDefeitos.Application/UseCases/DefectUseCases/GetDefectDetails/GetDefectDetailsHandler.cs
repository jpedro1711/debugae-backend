using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Domain.ViewModels;
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

            var consolidatedHistory = GetDefectHistoryViewModel(defectHistory.Select(dh => new DefectHistoryWithValueViewModel
            (
                dh.Action,
                dh.CreatedAt,
                dh.ContributorId,
                dh.OldMetadataJson,
                dh.NewMetadataJson
            )).ToList());

            var response = defectFullDetails with { History = consolidatedHistory };

            return new GetDefectDetailsResponse(response); 
        }

        private static List<DefectHistoryViewModel> GetDefectHistoryViewModel(
            List<DefectHistoryWithValueViewModel> defectHistoryWithValues)
        {
            var consolidatedHistory = new List<DefectHistoryViewModel>(defectHistoryWithValues.Count);

            foreach (var history in defectHistoryWithValues)
            {
                switch (history.Action)
                {
                    case DefectAction.Create:
                        consolidatedHistory.Add(new DefectHistoryViewModel(
                            DefectAction.Create.ToString(),
                            null,
                            null,
                            null,
                            history.ContributorId,
                            history.CreatedAt
                        ));
                        break;

                    case DefectAction.Update:
                        var differences = JsonComparer.CompareJson(
                            history.OldMetadataJson,
                            history.NewMetadataJson
                        );

                        if (differences != null && differences.Count > 0)
                        {
                            consolidatedHistory.AddRange(
                                differences.Select(dif => new DefectHistoryViewModel(
                                    DefectAction.Update.ToString(),
                                    dif.Field,
                                    dif.OldValue,
                                    dif.NewValue,
                                    history.ContributorId,
                                    history.CreatedAt
                                ))
                            );
                        }
                        break;
                }
            }

            return consolidatedHistory;
        }
    }
}
