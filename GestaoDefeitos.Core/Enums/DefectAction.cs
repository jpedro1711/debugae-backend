using System.ComponentModel;

namespace GestaoDefeitos.Domain.Enums
{
    public enum DefectAction
    {
        [Description("Create")]
        Create = 1,
        [Description("Update")]
        Update = 2,
        [Description("Delete")]
        Delete = 3,
    }
}
