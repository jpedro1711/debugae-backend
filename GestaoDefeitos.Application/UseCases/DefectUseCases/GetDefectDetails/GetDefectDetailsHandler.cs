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
            IDefectRepository defectRepository,
            ITagRepository tagRepository,
            IDefectHistoryRepository defectHistoryRepository,
            IDefectCommentRepository defectCommentRepository,
            IDefectRelationRepository defectRelationRepository,
            IDefectAttachmentRepository defectAttachmentRepository,
            ITrelloUserStoryRepository trelloUserStoryRepository,
            IContributorRepository contributorRepository,
            IProjectRepository projectRepository
        ) : IRequestHandler<GetDefectDetailsQuery, GetDefectDetailsResponse?>
    {
        public async Task<GetDefectDetailsResponse?> Handle(GetDefectDetailsQuery query, CancellationToken cancellationToken)
        {
            var defect = await defectRepository.GetByIdAsync(query.DefectId) ?? throw new ArgumentException("Invalid defect Id");

            var defectTags = await tagRepository.GetTagsByDefect(query.DefectId);

            var defectUserStories = await trelloUserStoryRepository.GetUserStoriesByDefectId(query.DefectId);

            var defectHistory = await defectHistoryRepository.GetDefectHistoryByDefectIdAsync(query.DefectId, cancellationToken);

            var defectComments = await defectCommentRepository.GetCommentsByDefectIdAsync(query.DefectId, cancellationToken);

            var attachment = await defectAttachmentRepository.GetAttachmentByDefectIdAsync(query.DefectId);

            var assignedContributor = await contributorRepository.GetByIdAsync(defect.AssignedToContributorId);

            var relatedDefects = await defectRelationRepository.GetRelationsByDefectIdAsync(query.DefectId, cancellationToken);

            var defectProject = await projectRepository.GetByIdAsync(defect.ProjectId);

            defect.TrelloUserStories = defectUserStories!;
            defect.DefectHistory = defectHistory!;
            defect.Comments = defectComments!;
            defect.Attachment = attachment;
            defect.RelatedDefects = relatedDefects!;
            defect.Tags = defectTags;
            defect.AssignedToContributor = assignedContributor!;
            defect.Project = defectProject!;

            return MapDefectToResponse(defect) ?? throw new ArgumentException("Failed to map defect to response");
        }

        private static string? GetCreatedByFullName(List<DefectHistory> defectHistory)
        {
            var creationHistory = defectHistory.FirstOrDefault(dh => dh.Action == DefectAction.Create);
            return creationHistory?.Contributor.FullName;
        }

        private static GetDefectDetailsResponse? MapDefectToResponse(Defect defect)
        {
            return new GetDefectDetailsResponse(
                    defect.Id,
                    defect.Description,
                    defect.Summary,
                    defect.CreatedAt,
                    GetCreatedByFullName(defect.DefectHistory),
                    defect.DefectSeverity.ToString(),
                    defect.Status.ToString(),
                    defect.ExpiresIn,
                    defect.DefectCategory.ToString(),
                    new DefectResponsibleContributorViewModel(
                        defect.AssignedToContributor.Id,
                        defect.AssignedToContributor.FullName
                    ),
                    new DefectDetailsViewModel(
                        defect.Description,
                        defect.DefectEnvironment.ToString(),
                        defect.ActualBehaviour,
                        defect.ExpectedBehaviour,
                        defect.Project.Name,
                        defect.AssignedToContributor.FullName
                    ),
                    defect.Comments.Select(c => new DefectCommentViewModel(
                        c.Contributor.FullName,
                        c.Content,
                        c.CreatedAt
                    )).ToList(),
                    defect.Attachment != null ? new DefectAttachmentViewModel(
                        defect.Attachment.FileName,
                        defect.Attachment.FileType,
                        defect.Attachment.CreatedAt,
                        defect.Attachment.UploadByUsername
                    ) : null,
                    defect.RelatedDefects.Select(rd => new DefectsSimplifiedViewModel(
                        rd.RelatedDefect.Id,
                        rd.RelatedDefect.Description,
                        rd.RelatedDefect.Summary,
                        rd.RelatedDefect.Status.ToString(),
                        rd.RelatedDefect.DefectPriority.ToString(),
                        rd.RelatedDefect.CreatedAt
                    )).ToList(),
                    GetDefectHistoryViewModel(defect.DefectHistory.Select(dh => new DefectHistoryWithValueViewModel(
                        dh.Action,
                        dh.CreatedAt,
                        dh.ContributorId,
                        dh.OldMetadataJson,
                        dh.NewMetadataJson
                    )).ToList()),
                    defect.TrelloUserStories.Select(ts => new TrelloUserStoryViewModel(
                        ts.Desc,
                        ts.ShortUrl,
                        ts.DefectId
                    )).ToList()
                );
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

    }
}
