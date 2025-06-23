using GestaoDefeitos.Domain.Enums;

namespace GestaoDefeitos.Domain.ViewModels
{
    public record DefectDetailsViewModel(string? DefectDescription, string DefectEnvironment, string? ActualBehaviour, string? ExpectedBehaviour, string? ProjectName, string? ResponsibleName);
}
