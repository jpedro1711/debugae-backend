using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.Reports.UserDefectsReport
{
    public class UserDefectsReportHandler(
            IDefectRepository defectRepository,
            AuthenticationContextAcessor authenticationContextAcessor
        ) : IRequestHandler<UserDefectsReportQuery, UserDefectsReportResponse?>
    {
        public async Task<UserDefectsReportResponse?> Handle(UserDefectsReportQuery request, CancellationToken cancellationToken)
        {
            var loggedUserId = authenticationContextAcessor.GetCurrentLoggedUserId();

            var defectsData = await defectRepository.GetDefectsDataByContributorIdAsync(loggedUserId);

            var metrics = new MetricsDto
            {
                Total = defectsData.Count,
                HighPriority = defectsData.Count(d => d.DefectPriority == DefectPriority.P5),
                InProgress = defectsData.Count(d => d.Status == DefectStatus.InProgress),
                Resolved = defectsData.Count(d => d.Status == DefectStatus.Resolved),
                New = defectsData.Count(d => d.Status == DefectStatus.New)
            };

            var statusData = Enum.GetValues(typeof(DefectStatus))
                .Cast<DefectStatus>()
                .Select(status => new StatusDataDto
                {
                    Name = status.ToString(), 
                    Value = defectsData.Count(d => d.Status == status)
                })
                .ToList();

            var severityData = Enum.GetValues(typeof(DefectSeverity))
                .Cast<DefectSeverity>()
                .Select(severity => new SeverityDataDto
                {
                    Name = severity.ToString(),
                    Value = defectsData.Count(d => d.DefectSeverity == severity)
                })
                .ToList();

            var categoryData = Enum.GetValues(typeof(DefectCategory))
                .Cast<DefectCategory>()
                .Select(category => new CategoryDataDto
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
                .Select(date => new {
                    date,
                    bugs = groupedByDate.ContainsKey(date) ? groupedByDate[date] : 0
                })
                .ToList();

            return new UserDefectsReportResponse
            {
                Metrics = metrics,
                StatusData = statusData,
                SeverityData = severityData,
                CategoryData = categoryData,
                TimelineData = timelineData.Select(td => new TimelineDataDto
                {
                    Date = td.date,
                    Bugs = td.bugs
                }).ToList()
            };
        }
    }
}
