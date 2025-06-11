using GestaoDefeitos.Domain.Entities.RelationEntities;
using GestaoDefeitos.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace GestaoDefeitos.Domain.Entities
{
    public class Contributor : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public ContributorProfession ContributorProfession { get; set; } = ContributorProfession.Developer;
        public List<ProjectContributor> ProjectContributors { get; set; } = [];
    }
}
