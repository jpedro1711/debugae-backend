using MediatR;

namespace GestaoDefeitos.Application.UseCases.Reports.GetContributorDefectsReport
{
    public record GetContributorDefectsReportRequest : IRequest<GetContributorDefectsReportResponse?>;
}
