using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.DownloadAttachment
{
    public class DownloadAttachmentHandler
        (
            IDefectAttachmentRepository defectAttachmentRepository
        ) : IRequestHandler<DownloadAttachmentCommand, DownloadAttachmentResponse?>
    {
        public async Task<DownloadAttachmentResponse?> Handle(DownloadAttachmentCommand request, CancellationToken cancellationToken)
        {
            var attachment = await defectAttachmentRepository.GetAttachmentByDefectIdAsync(request.DefectId);
            if (attachment == null)
                return null;

            return new DownloadAttachmentResponse(
                attachment.FileContent,
                attachment.FileName,
                attachment.FileType
            );
        }
    }
}
