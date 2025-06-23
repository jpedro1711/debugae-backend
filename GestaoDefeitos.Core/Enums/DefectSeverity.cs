using System.ComponentModel;

namespace GestaoDefeitos.Domain.Enums
{
    public enum DefectSeverity
    {
        [Description("Very high")]
        VeryHigh = 1,
        [Description("High")]
        High = 2,
        [Description("Medium")]
        Medium = 3,
        [Description("Low")]
        Low = 4,
        [Description("Very low")]
        VeryLow = 5
    }
}
