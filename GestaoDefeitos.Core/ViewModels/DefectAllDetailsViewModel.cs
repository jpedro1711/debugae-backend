namespace GestaoDefeitos.Domain.ViewModels
{
    public class DefectAllDetailsViewModel
    {
        public Guid? DefectId { get; set; }
        public string? DefectDescription { get; set; }
        public string? DefectSummary { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedByUser { get; set; }
        public string? DefectSeverity { get; set; }
        public string? DefectStatus { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? DefectCategory { get; set; }

        public DefectResponsibleContributorViewModel? ResponsibleContributor { get; set; }
        public DefectDetailsViewModel? Details { get; set; }
        public List<DefectCommentViewModel>? Comments { get; set; }
        public DefectAttachmentViewModel? Attachment { get; set; }
        public List<DefectsSimplifiedViewModel>? RelatedDefects { get; set; }

        public DefectAllDetailsViewModel() { }

        public DefectAllDetailsViewModel(
            Guid? defectId,
            string? defectDescription,
            string? defectSummary,
            DateTime? createdAt,
            string? createdByUser,
            string? defectSeverity,
            string? defectStatus,
            DateTime? expirationDate,
            string? defectCategory,
            DefectResponsibleContributorViewModel? responsibleContributor,
            DefectDetailsViewModel? details,
            List<DefectCommentViewModel>? comments,
            DefectAttachmentViewModel? attachment,
            List<DefectsSimplifiedViewModel>? relatedDefects)
        {
            DefectId = defectId;
            DefectDescription = defectDescription;
            DefectSummary = defectSummary;
            CreatedAt = createdAt;
            CreatedByUser = createdByUser;
            DefectSeverity = defectSeverity;
            DefectStatus = defectStatus;
            ExpirationDate = expirationDate;
            DefectCategory = defectCategory;
            ResponsibleContributor = responsibleContributor;
            Details = details;
            Comments = comments;
            Attachment = attachment;
            RelatedDefects = relatedDefects;
        }
    }
}
