using GestaoDefeitos.Application.TrelloIntegration.Responses;

namespace GestaoDefeitos.Application.TrelloIntegration
{
    public interface ITrelloIntegrationService
    {
        Task<List<TrelloWorkspaceViewModel>> GetWorkspacesAsync(string userId);
        Task<List<TrelloBoardViewModel>> GetBoardsAsync(string userId, string workspaceId);
        Task<List<TrelloCardViewModel>> GetCardsAsync(string userId, string boardId);
        Task<string> AddCommentAsync(string userId, string cardId, string comment, Guid defectId);
        void StoreAccessToken(string userId, string token, string tokenSecret);
        bool TryGetAccessToken(string userId, out (string token, string tokenSecret) tokenData);
        Task<string> GetLoginRedirectUrlAsync();
        Task<bool> HandleCallbackAsync(string oauth_token, string oauth_verifier, string userId);
    }
}
