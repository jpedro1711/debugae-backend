using System.ComponentModel;

namespace GestaoDefeitos.Domain.Enums
{
    public enum DefectEnvironment
    {
        [Description("Development")]
        Development = 1,
        [Description("Testing")]
        Staging = 2,
        [Description("Production")]
        Production = 3
    }
}
