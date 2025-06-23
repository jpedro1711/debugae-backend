namespace GestaoDefeitos.Domain.ViewModels.DefectsReport
{
    public class DefectMetricsViewModel
    {
        public int Total { get; set; }
        public int New { get; set; }
        public int InProgress { get; set; }
        public int Resolved { get; set; }
        public int HighPriority { get; set; }
    }
}
