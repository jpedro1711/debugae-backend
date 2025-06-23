using GestaoDefeitos.Domain.Enums;

namespace GestaoDefeitos.Domain.ViewModels
{
    public record DefectHistoryViewModel(
        string Action, 
        string? UpdatedField, 
        string? OldValue, 
        string? NewValue, 
        Guid ContributorId, 
        DateTime CreatedAt
    );
}
