namespace GestaoDefeitos.Domain.Entities
{
    using System.Text.Json.Serialization;

    public class TrelloCardDetails
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("desc")]
        public string? Description { get; set; }

        [JsonPropertyName("shortUrl")]
        public string? Url { get; set; }
    }

}
