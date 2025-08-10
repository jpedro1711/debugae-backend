using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Enums;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.DefectChangedEvent
{
    public record DefectChangeNotification : INotification
    {
        public Guid DefectId { get; init; }
        public Defect Defect { get; init; } = null!;
        public Guid ContributorId { get; init; }
        public Contributor Contributor { get; init; } = null!;
        public DefectAction Action { get; init; }
        public string? Field { get; init; } = null!;
        public string? OldValue { get; init; } = null!;
        public string? NewValue { get; init; } = null!;
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; init; }
    }
}
