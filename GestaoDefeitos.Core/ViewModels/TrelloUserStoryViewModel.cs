namespace GestaoDefeitos.Domain.ViewModels
{
    public record TrelloUserStoryViewModel
    (
       string? Desc,
       string? ShortUrl,
       Guid DefectId
    );
}
