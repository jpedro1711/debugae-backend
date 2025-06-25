using GestaoDefeitos.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace GestaoDefeitos.Domain.Entities
{
    public class Contributor : IdentityUser<Guid>
    {
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string FullName => $"{Firstname} {Lastname}";
        public ContributorProfession ContributorProfession { get; set; }
        public List<ProjectContributor> ProjectContributors { get; set; } = [];
    }
}
