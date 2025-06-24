namespace GestaoDefeitos.Application.TrelloIntegration
{
    public interface ITrelloIntegrationService
    {
        Task<string> GetWorkspacesAsync(string userId);
        Task<string> GetBoardsAsync(string userId, string workspaceId);
        Task<string> GetCardsAsync(string userId, string boardId);
        Task<string> AddCommentAsync(string userId, string cardId, string comment);
        void StoreAccessToken(string userId, string token, string tokenSecret);
        bool TryGetAccessToken(string userId, out (string token, string tokenSecret) tokenData);
        Task<string> GetLoginRedirectUrlAsync();
        Task<bool> HandleCallbackAsync(string oauth_token, string oauth_verifier, string userId);
    }
}
