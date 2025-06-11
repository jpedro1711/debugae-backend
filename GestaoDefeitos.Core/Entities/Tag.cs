namespace GestaoDefeitos.Domain.Entities
{
    public class Tag
    {
        public Guid Id { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Tag() { }

        public Tag(Guid id, string description)
        {
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required");

            Id = id;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required");
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
