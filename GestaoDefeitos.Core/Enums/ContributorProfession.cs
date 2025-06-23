using System.ComponentModel;

namespace GestaoDefeitos.Domain.Enums
{
    public enum ContributorProfession
    {
        [Description("Developer")]
        Developer = 1,
        [Description("Tester")]
        Tester = 2,
        [Description("Product Owner")]
        ProductOwner = 3,
        [Description("Scrum Master")]
        ScrumMaster = 4,
        [Description("Business Analyst")]
        BusinessAnalyst = 5,
        Architect = 6,
        [Description("DevOps")]
        DevOps = 7,
        [Description("Designer")]
        Designer = 8,
        [Description("Project Manager")]
        ProjectManager = 9,
        [Description("Other")]
        Other = 10
    }
}
