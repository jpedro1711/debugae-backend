using System.ComponentModel;

namespace GestaoDefeitos.Domain.Enums
{
    public enum DefectStatus
    {
        [Description("Resolved")]
        Resolved = 1,
        [Description("Invalid")]
        Invalid = 2,
        [Description("Reopened")]
        Reopened = 3,
        [Description("In Progress")]
        InProgress = 4,
        [Description("Waiting for User")]
        WaitingForUser = 5,
        [Description("New")]
        New = 6
    }
}
