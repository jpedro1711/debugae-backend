using MediatR;

namespace GestaoDefeitos.Application.UseCases.Reports.ProjectDefectReport
{
    public record ProjectDefectsReportQuery(Guid ProjectId, DateTime? InitialDate, DateTime? FinalDate) : IRequest<ProjectDefectsReportResponse?>;
}
