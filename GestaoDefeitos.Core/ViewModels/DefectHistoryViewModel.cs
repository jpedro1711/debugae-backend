namespace GestaoDefeitos.Domain.ViewModels
{
    public record DefectHistoryViewModel(string Action, string? UpdatedField, string? OldValue, string? NewValue, string ContributorId, DateTime Date);
}
