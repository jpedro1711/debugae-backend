using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.NotifyDefectMailLetter
{
    public class NotifyDefectMailLetterNotification : INotification
    {
        public Guid DefectId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
