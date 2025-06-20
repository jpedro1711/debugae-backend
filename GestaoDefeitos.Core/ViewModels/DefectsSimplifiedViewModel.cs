using GestaoDefeitos.Domain.Enums;

namespace GestaoDefeitos.Domain.ViewModels
{
    public record DefectsSimplifiedViewModel(
            Guid Id,
            string Description,
            string Summary,
            DefectStatus Status,
            DefectPriority DefectPriority,
            DateTime CreatedAt
        );
}
