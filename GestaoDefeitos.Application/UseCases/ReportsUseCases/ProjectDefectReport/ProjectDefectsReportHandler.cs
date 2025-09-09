using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Domain.ViewModels.DefectsReport;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.Reports.ProjectDefectReport
{
    public class ProjectDefectsReportHandler
        (
            IDefectRepository defectRepository
        ) : IRequestHandler<ProjectDefectsReportQuery, ProjectDefectsReportResponse?>
    {
        public async Task<ProjectDefectsReportResponse?> Handle(
            ProjectDefectsReportQuery request,
            CancellationToken cancellationToken)
        {
            var defectsData = await defectRepository.GetDefectsDataByProjectIdAsync(request.ProjectId);

            var metrics = new DefectMetricsViewModel
            {
                Total = defectsData.Count,
                HighPriority = defectsData.Count(d => d.DefectPriority == DefectPriority.P5),
                InProgress = defectsData.Count(d => d.Status == DefectStatus.InProgress),
                Resolved = defectsData.Count(d => d.Status == DefectStatus.Resolved),
                New = defectsData.Count(d => d.Status == DefectStatus.New)
            };

            var statusData = Enum.GetValues(typeof(DefectStatus))
                .Cast<DefectStatus>()
                .Select(status => new DefectByStatusViewModel
                {
                    Name = status.ToString(),
                    Value = defectsData.Count(d => d.Status == status)
                })
                .ToList();

            var severityData = Enum.GetValues(typeof(DefectSeverity))
                .Cast<DefectSeverity>()
                .Select(severity => new DefectBySeverityViewModel
                {
                    Name = severity.ToString(),
                    Value = defectsData.Count(d => d.DefectSeverity == severity)
                })
                .ToList();

            var categoryData = Enum.GetValues(typeof(DefectCategory))
                .Cast<DefectCategory>()
                .Select(category => new DefectByCategoryViewModel
                {
                    Category = category.ToString(),
                    Count = defectsData.Count(d => d.DefectCategory == category)
                })
                .ToList();

            var groupedByDate = defectsData
                .GroupBy(b => b.CreatedAt.ToString("dd/MM"))
                .ToDictionary(g => g.Key, g => g.Count());

            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Today.AddDays(-i).ToString("dd/MM"))
                .ToList();

            var timelineData = last7Days
                .Select(date => new
                {
                    date,
                    defects = groupedByDate.TryGetValue(date, out var count) ? count : 0
                })
                .ToList();

            return new ProjectDefectsReportResponse
            {
                Metrics = metrics,
                StatusData = statusData,
                SeverityData = severityData,
                CategoryData = categoryData,
                TimelineData = timelineData.Select(td => new DefectsTimeLineDataViewModel
                {
                    Date = td.date,
                    Defects = td.defects
                }).ToList()
            };
        }
    }
}
