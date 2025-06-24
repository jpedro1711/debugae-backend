namespace GestaoDefeitos.WebApi.Cache
{
    public interface ITrelloRequestTokenCache
    {
        void StoreAccessToken(string userId, string token, string tokenSecret);
        (string token, string tokenSecret)? GetAccessToken(string userId);

        void StoreRequestToken(string requestToken, string tokenSecret);
        string? GetRequestTokenSecret(string requestToken);
    }

}
