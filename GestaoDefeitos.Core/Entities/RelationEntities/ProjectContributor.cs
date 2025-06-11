using GestaoDefeitos.Domain.Enums;

namespace GestaoDefeitos.Domain.Entities.RelationEntities
{
    public class ProjectContributor
    {
        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; }
        public Guid ContributorId { get; private set; }
        public Contributor Contributor { get; private set; }
        public ProjectRole Role { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private ProjectContributor() { }

        public ProjectContributor(Guid projectId, Project project, Guid contributorId, Contributor contributor, ProjectRole role)
        {
            ArgumentNullException.ThrowIfNull(project);
            ArgumentNullException.ThrowIfNull(contributor);

            ProjectId = projectId;
            Project = project;
            ContributorId = contributorId;
            Contributor = contributor;
            Role = role;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateRole(ProjectRole role)
        {
            if (Role != role)
            {
                Role = role;
                UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
