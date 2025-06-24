using GestaoDefeitos.Application.TrelloIntegration;
using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetDefectDetails
{
    public record GetDefectDetailsResponse(
            Guid? DefectId,
            string? DefectDescription,
            string? DefectSummary,
            DateTime? CreatedAt,
            string? CreatedByUser,
            DefectSeverity? DefectSeverity,
            DefectStatus? DefectStatus,
            DateTime? ExpirationDate,
            DefectCategory? DefectCategory,
            DefectResponsibleContributorViewModel? ResponsibleContributor,
            DefectDetailsViewModel? Details,
            List<DefectCommentViewModel>? Comments,
            DefectAttachmentViewModel? Attachment,
            List<DefectsSimplifiedViewModel>? RelatedDefects,
            List<DefectHistoryViewModel> History,
            List<TrelloUserStory> TrelloUserStories
        );
}
