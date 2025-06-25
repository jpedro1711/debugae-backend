using MediatR;

namespace GestaoDefeitos.Application.UseCases.Reports.GetProjectDefectsReport
{
    public record GetProjectDefectsReportRequest(Guid ProjectId) : IRequest<GetProjectDefectsReportResponse?>;
}
