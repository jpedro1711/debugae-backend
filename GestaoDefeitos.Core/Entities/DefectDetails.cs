using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Domain.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace GestaoDefeitos.Domain.Entities
{
    [Keyless]
    public class DefectDetailsView
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public string? Summary { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DefectStatus? Status { get; set; }
        public DefectSeverity? DefectSeverity { get; set; }
        public DateTime? ExpiresIn { get; set; }
        public DefectCategory DefectCategory { get; set; }
        public DefectEnvironment DefectEnvironment { get; set; }
        public string? ProjectName { get; set; }
        public Guid AssignedTo { get; set; }
        public string? ContributorName { get; set; }
        public string? AttachmentFileName { get; set; }
        public string? AttachmentFileType { get; set; }
        public DateTime? AttachmentCreatedAt { get; set; }
        public string? UploadByUsername { get; set; }
        public string? CommentsJson { get; set; }
        public string? RelatedDefectsJson { get; set; }
        public string? HistoryJson { get; set; }
        public string? ActualBehaviour { get; set; }
        public string? ExpectedBehaviour { get; set; }

        [NotMapped]
        public List<DefectCommentViewModel> Comments =>
            string.IsNullOrEmpty(CommentsJson)
                ? []
                : JsonSerializer.Deserialize<List<DefectCommentViewModel>>(CommentsJson!)!;

        [NotMapped]
        public List<DefectsSimplifiedViewModel> RelatedDefects =>
            string.IsNullOrEmpty(RelatedDefectsJson)
                ? []
                : JsonSerializer.Deserialize<List<DefectsSimplifiedViewModel>>(RelatedDefectsJson!)!;

        [NotMapped]
        public List<DefectHistoryWithValueViewModel> History =>
            string.IsNullOrEmpty(HistoryJson)
                ? []
                : JsonSerializer.Deserialize<List<DefectHistoryWithValueViewModel>>(HistoryJson!)!;
    }
}
