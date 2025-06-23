using System.ComponentModel;

namespace GestaoDefeitos.Domain.Enums
{
    public enum DefectCategory
    {
        [Description("Functional")]
        Functional = 1,
        [Description("Interface")]
        Interface = 2,
        [Description("Performance")]
        Performance = 3,
        [Description("Improvement")]
        Improvement = 4
    }
}
