namespace GestaoDefeitos.Application.TrelloIntegration
{
    public class TrelloApiOptions
    {
        public string ConsumerKey { get; set; } = string.Empty;
        public string ConsumerSecret { get; set; } = string.Empty;
        public string GetWorkspacesUrl { get; set; } = string.Empty;
        public string GetBoardsUrl { get; set; } = string.Empty;
        public string GetCardsUrl { get; set; } = string.Empty;
        public string AddCommentUrl { get; set; } = string.Empty;
        public string CallbackUrl { get; set; } = string.Empty;
        public string OAuthAuthorizeTokenUrl { get; set; } = string.Empty;
        public string OAuthGetRequestTokenUrl { get; set; } = string.Empty;
        public string OAuthGetAccessTokenUrl { get; set; } = string.Empty;
    }
}
