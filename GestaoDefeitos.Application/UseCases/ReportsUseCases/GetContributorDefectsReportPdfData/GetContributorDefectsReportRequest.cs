using MediatR;

namespace GestaoDefeitos.Application.UseCases.Reports.GetContributorDefectsReportPdfData
{
    public record GetContributorDefectsReportRequest : IRequest<GetContributorDefectsReportResponse?>;
}
