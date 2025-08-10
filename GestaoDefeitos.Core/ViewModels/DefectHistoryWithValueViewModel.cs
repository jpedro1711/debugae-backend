using GestaoDefeitos.Domain.Enums;

namespace GestaoDefeitos.Domain.ViewModels
{
    public record DefectHistoryWithValueViewModel(
            DefectAction Action,
            DateTime CreatedAt,
            Guid ContributorId,
            string? UpdatedField,
            string? OldValue,
            string? NewValue
        );
}
