namespace GestaoDefeitos.Domain.Entities
{
    public class TrelloUserStory
    {
        public Guid? Id { get; set; }
        public string? Desc { get; set; }
        public string? ShortUrl { get; set; }
        public Guid DefectId { get; set; }
        public Defect? Defect { get; set; }
    }

}
