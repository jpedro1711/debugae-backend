namespace GestaoDefeitos.Domain.ViewModels
{
    public record TrelloUserStoryViewModel
    (
       string? Desc,
       string? ShortUrl,
       string? Name,
       Guid DefectId
    );
}
