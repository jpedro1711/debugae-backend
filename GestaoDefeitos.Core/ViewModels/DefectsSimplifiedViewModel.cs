namespace GestaoDefeitos.Domain.ViewModels
{
    public record DefectsSimplifiedViewModel(
            string DefectId,
            string DefectDescription,
            string DefectSummary,
            string DefectStatus,
            string DefectPriority,
            DateTime CreatedAt
        );
}
