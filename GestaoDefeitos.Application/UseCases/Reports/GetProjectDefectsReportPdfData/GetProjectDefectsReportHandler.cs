using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.Reports.GetProjectDefectsReportPdfData
{
    public class GetProjectDefectsReportHandler(
        IDefectRepository defectRepository,
        IProjectRepository projectRepository
        ) : IRequestHandler<GetProjectDefectsReportRequest, GetProjectDefectsReportResponse?>
    {
        public async Task<GetProjectDefectsReportResponse?> Handle(
            GetProjectDefectsReportRequest request,
            CancellationToken cancellationToken)
        {
            var project = await projectRepository.GetByIdAsync(request.ProjectId)
                ?? throw new InvalidOperationException("Project not found. Please check the project ID.");
            var defects = await defectRepository.GetDefectsByProjectAsync(request.ProjectId, cancellationToken);
            return new GetProjectDefectsReportResponse(defects, project.Name);
        }
    }
}
