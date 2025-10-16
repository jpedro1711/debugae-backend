using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Domain.ViewModels.DefectsReport;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GestaoDefeitos.Application.UseCases.Reports.ProjectDefectReport
{
    public class ProjectDefectsReportHandler
        (
            IDefectRepository defectRepository,
            IDefectHistoryRepository defectHistoryRepository
        ) : IRequestHandler<ProjectDefectsReportQuery, ProjectDefectsReportResponse?>
    {
        public async Task<ProjectDefectsReportResponse?> Handle(
            ProjectDefectsReportQuery request,
            CancellationToken cancellationToken)
        {
            var defectsDataByProject = defectRepository.GetDefectsDataByProjectIdAsync(request.ProjectId);

            if (request.InitialDate.HasValue && request.FinalDate.HasValue)
            {
                var initialDate = request.InitialDate.Value.Date;
                var finalDate = request.FinalDate.Value.Date.AddDays(1).AddTicks(-1);

                defectsDataByProject = defectsDataByProject.Where(d => d.CreatedAt >= initialDate && d.CreatedAt <= finalDate);
            }
            else if (request.InitialDate.HasValue)
            {
                var initialDate = request.InitialDate.Value.Date;
                defectsDataByProject = defectsDataByProject.Where(d => d.CreatedAt >= initialDate);
            }
            else if (request.FinalDate.HasValue)
            {
                var finalDate = request.FinalDate.Value.Date.AddDays(1).AddTicks(-1); 
                defectsDataByProject = defectsDataByProject.Where(d => d.CreatedAt <= finalDate);
            }

            var defectsData = await defectsDataByProject.ToListAsync(cancellationToken);

            if (defectsData is null || defectsData.Count == 0)
            {
                var emptyMetrics = new DefectMetricsViewModel
                {
                    HighPriority = 0,
                    InProgress = 0,
                    New = 0,
                    Resolved = 0,
                    Total = 0
                };

                return new ProjectDefectsReportResponse
                {
                    Metrics = emptyMetrics,
                    StatusData = new(),
                    SeverityData = new(),
                    CategoryData = new(),
                    TimelineData = new(),
                    ResolutionIndex = 0,
                    DefectByVersion = Enumerable.Empty<DefectByVersionViewModel>(),
                    DefectResolutionAverageTimeInDays = 0
                };
            }

            var metrics = new DefectMetricsViewModel
            {
                Total = defectsData.Count,
                HighPriority = defectsData.Count(d => d.DefectPriority == DefectPriority.P5),
                InProgress = defectsData.Count(d => d.Status == DefectStatus.InProgress),
                Resolved = defectsData.Count(d => d.Status == DefectStatus.Resolved),
                New = defectsData.Count(d => d.Status == DefectStatus.New)
            };

            decimal resolvedDefectsCount = defectsData.Count(d => d.Status == DefectStatus.Resolved);
            decimal invalidDefectsCount = defectsData.Count(d => d.Status == DefectStatus.Invalid);

            decimal resolutionIndex = ((resolvedDefectsCount - invalidDefectsCount) / defectsData.Count) * 100;

            decimal invalidDefectIndex = (defectsData.Count == 0) ? 0 : (invalidDefectsCount / defectsData.Count) * 100;

            IEnumerable<DefectByVersionViewModel> defectByVersion = defectsData
                .GroupBy(d => d.Version)
                .Select(d => new DefectByVersionViewModel
                {
                    Name = d.Key,
                    Value = d.Count()
                });

            double defectResolutionAverageTimeInDays = defectsData.Count == 0 ? 0 : defectsData
                .Where(d => d.Status == DefectStatus.Resolved)
                .Select(d =>
                {
                    var history = defectHistoryRepository.GetDefectHistoryByDefectIdAsync(d.Id, cancellationToken).Result;
                    var resolvedEntry = history
                        .FirstOrDefault(h => h.NewValue == DefectStatus.Resolved.ToString());
                    if (resolvedEntry != null)
                    {
                        return (resolvedEntry.CreatedAt - d.CreatedAt).TotalDays;
                    }
                    return 0;
                })
                .DefaultIfEmpty(0)
                .Average();

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
                }).ToList(),
                ResolutionIndex = Math.Round(resolutionIndex, 2),
                DefectByVersion = defectByVersion,
                DefectResolutionAverageTimeInDays = Math.Round(defectResolutionAverageTimeInDays, 2),
                InvalidDefectsPercentage = Math.Round(invalidDefectIndex, 2)
            };
        }
    }
}
