using MediatR;

namespace GestaoDefeitos.Application.UseCases.Reports.ProjectDefectReport
{
    public record ProjectDefectsReportQuery(Guid ProjectId) : IRequest<ProjectDefectsReportResponse?>;
}
