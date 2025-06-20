using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.DownloadAttachment
{
    public record DownloadAttachmentCommand(Guid DefectId) : IRequest<DownloadAttachmentResponse?>;
}
