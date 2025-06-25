using System.Text.Json.Serialization;

namespace GestaoDefeitos.Application.TrelloIntegration.Responses
{
    public class TrelloWorkspaceViewModel
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("displayName")]
        public string? Name { get; set; }
    }
}
