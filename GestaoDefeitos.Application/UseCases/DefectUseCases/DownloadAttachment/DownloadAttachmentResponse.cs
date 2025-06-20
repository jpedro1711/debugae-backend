namespace GestaoDefeitos.Application.UseCases.DefectUseCases.DownloadAttachment
{
    public record DownloadAttachmentResponse(
        byte[] Content,
        string FileName,
        string ContentType
    );
}
