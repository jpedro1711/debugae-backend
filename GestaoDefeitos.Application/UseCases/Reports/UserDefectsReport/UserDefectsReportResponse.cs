namespace GestaoDefeitos.Application.UseCases.Reports.UserDefectsReport
{
    public class UserDefectsReportResponse
    {
        public MetricsDto? Metrics { get; set; }
        public List<StatusDataDto>? StatusData { get; set; }
        public List<SeverityDataDto>? SeverityData { get; set; }
        public List<CategoryDataDto>? CategoryData { get; set; }
        public List<TimelineDataDto>? TimelineData { get; set; }
    }

    public class MetricsDto
    {
        public int Total { get; set; }
        public int New { get; set; }
        public int InProgress { get; set; }
        public int Resolved { get; set; }
        public int HighPriority { get; set; }
    }

    public class StatusDataDto
    {
        public string? Name { get; set; }
        public int Value { get; set; }
    }

    public class SeverityDataDto
    {
        public string? Name { get; set; }
        public int Value { get; set; }
    }

    public class CategoryDataDto
    {
        public string? Category { get; set; }
        public int Count { get; set; }
    }

    public class TimelineDataDto
    {
        public string? Date { get; set; }
        public int Bugs { get; set; }
    }

}
