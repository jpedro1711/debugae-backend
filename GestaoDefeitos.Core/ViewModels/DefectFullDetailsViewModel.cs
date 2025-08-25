namespace GestaoDefeitos.Domain.ViewModels
{
    public record DefectFullDetailsViewModel
    (
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
        IEnumerable<DefectCommentViewModel>? Comments,
        DefectAttachmentViewModel? Attachment,
        IEnumerable<DefectsSimplifiedViewModel>? RelatedDefects,
        IEnumerable<DefectHistoryViewModel>? History,
        IEnumerable<TrelloUserStoryViewModel> TrelloUserStories,
        string LogStackTrace
    );
}
