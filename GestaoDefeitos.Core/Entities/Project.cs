using GestaoDefeitos.Domain.Entities.RelationEntities;

namespace GestaoDefeitos.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<ProjectContributor> ProjectContributors { get; private set; } = [];
        public List<Defect> Defects { get; private set; } = [];
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Project() { }

        public Project(Guid id, string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required");
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required");

            Id = id;
            Name = name;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required");
            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required");
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddContributor(ProjectContributor contributor)
        {
            ArgumentNullException.ThrowIfNull(contributor);
            if (!ProjectContributors.Contains(contributor))
            {
                ProjectContributors.Add(contributor);
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void RemoveContributor(ProjectContributor contributor)
        {
            ArgumentNullException.ThrowIfNull(contributor);
            if (ProjectContributors.Remove(contributor))
            {
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void AddDefect(Defect defect)
        {
            ArgumentNullException.ThrowIfNull(defect);
            if (!Defects.Contains(defect))
            {
                Defects.Add(defect);
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void RemoveDefect(Defect defect)
        {
            ArgumentNullException.ThrowIfNull(defect);
            if (Defects.Remove(defect))
            {
                UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
