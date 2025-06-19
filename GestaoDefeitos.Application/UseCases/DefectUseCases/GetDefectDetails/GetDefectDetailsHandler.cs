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
            var defectDetails = await defectRepository.GetDefectDetailsProjectionAsync(query.DefectId, cancellationToken);

            var changeHistory = await CreateDefectHistoryViewModel(
                query.DefectId,
                defectHistoryRepository,
                cancellationToken
            );

            return CreateDefectDetailsResponse(defectDetails, changeHistory);
        }

        private async static Task<List<DefectHistoryViewModel>> CreateDefectHistoryViewModel(Guid defectId, IDefectHistoryRepository defectHistoryRepository, CancellationToken cancellationToken)
        {
            var consolidatedHistory = new List<DefectHistoryViewModel>();
            var defectHistories = await defectHistoryRepository.GetDefectHistoryByDefectIdAsync(defectId, cancellationToken);

            foreach (var history in defectHistories)
            {
                if (history.Action == DefectAction.Create)
                    consolidatedHistory.Add(new DefectHistoryViewModel(
                        DefectAction.Create.ToString(),
                        null,
                        null,
                        null,
                        history.Contributor.Id.ToString(),
                        history.CreatedAt
                    ));
                else if (history.Action == DefectAction.Update)
                {
                    var differences = JsonComparer.CompareJson(
                        history.OldMetadataJson,
                        history.NewMetadataJson
                    );

                    differences.Select(dif => new DefectHistoryViewModel(
                        DefectAction.Update.ToString(),
                        dif.Field,
                        dif.OldValue,
                        dif.NewValue,
                        history.Contributor.Id.ToString(),
                        history.CreatedAt
                    )).ToList().ForEach(dif => consolidatedHistory.Add(dif));
                }
            }

            return consolidatedHistory;
        }

        private static GetDefectDetailsResponse? CreateDefectDetailsResponse(DefectAllDetailsViewModel? defect, List<DefectHistoryViewModel> changeHistory)
        {

            return new GetDefectDetailsResponse(
                    defect?.DefectId,
                    defect?.DefectDescription,
                    defect?.DefectSummary,
                    defect?.CreatedAt,
                    defect?.CreatedByUser,
                    defect?.DefectSeverity,
                    defect?.DefectStatus,
                    defect?.ExpirationDate,
                    defect?.DefectCategory,
                    defect?.ResponsibleContributor,
                    defect?.Details,
                    defect?.Comments,
                    defect?.Attachment,
                    defect?.RelatedDefects,
                    changeHistory
                );
        }
    }
}
