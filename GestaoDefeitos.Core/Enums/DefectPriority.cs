using System.ComponentModel;

namespace GestaoDefeitos.Domain.Enums
{
    public enum DefectPriority
    {
        [Description("P1 - Too high")]
        P1 = 1,
        [Description("P2 - High")]
        P2 = 2,
        [Description("P3 - Medium")]
        P3 = 3,
        [Description("P4 - Low")]
        P4 = 4,
        [Description("P5 - Too low")]
        P5 = 5
    }
}
