using GestaoDefeitos.Domain.Entities.RelationEntities;
using GestaoDefeitos.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace GestaoDefeitos.Domain.Entities
{
    public class Contributor : IdentityUser<Guid>
    {
        public string FullName { get; private set; }
        public ContributorProfession ContributorProfession { get; private set; }
        public List<ProjectContributor> ProjectContributors { get; private set; } = [];

        private Contributor() { }

        public Contributor(Guid id, string userName, string fullName, ContributorProfession profession)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException("UserName is required");
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("FullName is required");

            Id = id;
            UserName = userName;
            FullName = fullName;
            ContributorProfession = profession;
        }

        public void UpdateFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("FullName is required");
            FullName = fullName;
        }

        public void UpdateProfession(ContributorProfession profession)
        {
            ContributorProfession = profession;
        }

        public void AddProjectContributor(ProjectContributor projectContributor)
        {
            ArgumentNullException.ThrowIfNull(projectContributor);
            if (!ProjectContributors.Contains(projectContributor))
            {
                ProjectContributors.Add(projectContributor);
            }
        }

        public void RemoveProjectContributor(ProjectContributor projectContributor)
        {
            ArgumentNullException.ThrowIfNull(projectContributor);
            ProjectContributors.Remove(projectContributor);
        }
    }
}
