using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetDefectDetails
{
    public record GetDefectDetailsResponse(
            Guid? DefectId,
            string? DefectDescription,
            string? DefectSummary,
            DateTime? CreatedAt,
            string? CreatedByUser,
            string? DefectSeverity,
            string? DefectStatus,
            DateTime? ExpirationDate,
            string? DefectCategory,
            DefectResponsibleContributorViewModel? ResponsibleContributor,
            DefectDetailsViewModel? Details,
            List<DefectCommentViewModel>? Comments,
            DefectAttachmentViewModel? Attachment,
            List<DefectsSimplifiedViewModel>? RelatedDefects,
            List<DefectHistoryViewModel> History
        );
}
