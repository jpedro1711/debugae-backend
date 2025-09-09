using System.Text.Json.Serialization;

namespace GestaoDefeitos.Application.TrelloIntegration.Responses
{
    public class TrelloCardViewModel
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("desc")]
        public string? Description { get; set; }
        [JsonPropertyName("shortUrl")]
        public string? Url { get; set; }
    }
}
