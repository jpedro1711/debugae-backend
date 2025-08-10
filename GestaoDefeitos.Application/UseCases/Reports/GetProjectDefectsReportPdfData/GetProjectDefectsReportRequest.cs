using MediatR;

namespace GestaoDefeitos.Application.UseCases.Reports.GetProjectDefectsReportPdfData
{
    public record GetProjectDefectsReportRequest(Guid ProjectId) : IRequest<GetProjectDefectsReportResponse?>;
}
