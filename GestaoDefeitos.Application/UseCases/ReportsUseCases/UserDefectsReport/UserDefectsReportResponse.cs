using GestaoDefeitos.Domain.ViewModels.DefectsReport;

namespace GestaoDefeitos.Application.UseCases.Reports.UserDefectsReport
{
    public class UserDefectsReportResponse
    {
        public DefectMetricsViewModel? Metrics { get; set; }
        public List<DefectByStatusViewModel>? StatusData { get; set; }
        public List<DefectBySeverityViewModel>? SeverityData { get; set; }
        public List<DefectByCategoryViewModel>? CategoryData { get; set; }
        public List<DefectsTimeLineDataViewModel>? TimelineData { get; set; }
        public decimal ResolutionIndex { get; set; }
        public IEnumerable<DefectByVersionViewModel>? DefectByVersion { get; set; }
        public double DefectResolutionAverageTimeInDays { get; set; }
        public decimal InvalidDefectsPercentage { get; set; }
    }

}
