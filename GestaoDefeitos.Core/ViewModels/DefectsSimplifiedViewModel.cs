namespace GestaoDefeitos.Domain.ViewModels
{
    public record DefectsSimplifiedViewModel(
            Guid Id,
            string Description,
            string Summary,
            string Status,
            string DefectPriority,
            DateTime ExpirationDate,
            DateTime CreatedAt,
            string Category,
            ProjectSimplifiedViewModel Project
        );
}
