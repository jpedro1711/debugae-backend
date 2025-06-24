using GestaoDefeitos.Application.TrelloIntegration;
using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Domain.ViewModels;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetDefectDetails
{
    public class GetDefectDetailsHandler
        (
            IDefectDetailsViewRepository defectDetailsViewRepository,
            ITrelloUserStoryRepository trelloUserStoryRepository
        ) : IRequestHandler<GetDefectDetailsQuery, GetDefectDetailsResponse?>
    {
        public async Task<GetDefectDetailsResponse?> Handle(GetDefectDetailsQuery query, CancellationToken cancellationToken)
        {
            var defectDetails = await defectDetailsViewRepository.GetDefectDetails(query.DefectId, cancellationToken);

            var userStories = await trelloUserStoryRepository.GetUserStoriesByDefectId(query.DefectId);

            if (defectDetails == null)
                return null;

            return MapToDefectDetailsResponse(defectDetails, userStories);
        }

        private static List<DefectHistoryViewModel> GetDefectHistoryViewModel(List<DefectHistoryWithValueViewModel> defectHistoryWithValues)
        {
            List<DefectHistoryViewModel> consolidatedHistory = [];

            foreach (var history in defectHistoryWithValues)
            {
                if (history.Action == DefectAction.Create)
                    consolidatedHistory.Add(new DefectHistoryViewModel(
                        DefectAction.Create.ToString(),
                        null,
                        null,
                        null,
                        history.ContributorId,
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
                        history.ContributorId,
                        history.CreatedAt
                    )).ToList().ForEach(dif => consolidatedHistory.Add(dif));
                }
            }

            return consolidatedHistory;
        }

        private static GetDefectDetailsResponse? MapToDefectDetailsResponse(DefectDetailsView? defectDetailsView, List<TrelloUserStory?> trelloUserStories)
        {
            if (defectDetailsView is null)
                return null;

            return new GetDefectDetailsResponse(
                    defectDetailsView.Id,
                    defectDetailsView.Description,
                    defectDetailsView.Summary,
                    defectDetailsView.CreatedAt,
                    defectDetailsView.CreatedBy,
                    defectDetailsView.DefectSeverity,
                    defectDetailsView.Status,
                    defectDetailsView.ExpiresIn,
                    defectDetailsView.DefectCategory,
                    new DefectResponsibleContributorViewModel
                    (
                        defectDetailsView.AssignedTo,
                        defectDetailsView.ContributorName
                    ),
                    new DefectDetailsViewModel
                    (
                        defectDetailsView.Description,
                        defectDetailsView.DefectEnvironment.ToString(),
                        defectDetailsView.ActualBehaviour,
                        defectDetailsView.ExpectedBehaviour,
                        defectDetailsView.ProjectName,
                        defectDetailsView.ContributorName
                    ),
                    defectDetailsView.Comments,
                    new DefectAttachmentViewModel
                    (
                        defectDetailsView.AttachmentFileName,
                        defectDetailsView.AttachmentFileType,
                        defectDetailsView.AttachmentCreatedAt,
                        defectDetailsView.UploadByUsername
                    ),
                    defectDetailsView.RelatedDefects,
                    GetDefectHistoryViewModel(defectDetailsView.History),
                    trelloUserStories
                );
        }

    }
}
