using System.Text.Json.Serialization;

namespace GestaoDefeitos.Application.TrelloIntegration.Responses
{
    public class TrelloBoardViewModel
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
