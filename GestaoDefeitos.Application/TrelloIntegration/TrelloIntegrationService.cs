using GestaoDefeitos.WebApi.Cache;
using OAuth;
using Microsoft.Extensions.Options;

namespace GestaoDefeitos.Application.TrelloIntegration
{
    public class TrelloIntegrationService
        (
            ITrelloRequestTokenCache cache,
            IOptions<TrelloApiOptions> options
        ) : ITrelloIntegrationService
    {
        private readonly ITrelloRequestTokenCache _cache = cache;
        private readonly TrelloApiOptions _options = options.Value;

        public void StoreAccessToken(string userId, string token, string tokenSecret)
        {
            _cache.StoreAccessToken(userId, token, tokenSecret);
        }

        public bool TryGetAccessToken(string userId, out (string token, string tokenSecret) tokenData)
        {
            var result = _cache.GetAccessToken(userId);
            if (result == null)
            {
                tokenData = default;
                return false;
            }

            tokenData = result.Value;
            return true;
        }

        public async Task<string> GetWorkspacesAsync(string userId)
        {
            var (token, secret) = EnsureToken(userId);
            return await TrelloGetAsync(_options.GetWorkspacesUrl, token, secret);
        }

        public async Task<string> GetBoardsAsync(string userId, string workspaceId)
        {
            var (token, secret) = EnsureToken(userId);
            var url = string.Format(_options.GetBoardsUrl, workspaceId);
            return await TrelloGetAsync(url, token, secret);
        }

        public async Task<string> GetCardsAsync(string userId, string boardId)
        {
            var (token, secret) = EnsureToken(userId);
            var url = string.Format(_options.GetCardsUrl, boardId);
            return await TrelloGetAsync(url, token, secret);
        }

        public async Task<string> AddCommentAsync(string userId, string cardId, string comment)
        {
            var (token, secret) = EnsureToken(userId);
            var url = string.Format(_options.AddCommentUrl, cardId, Uri.EscapeDataString(comment));
            return await TrelloPostAsync(url, token, secret);
        }

        public async Task<string> GetLoginRedirectUrlAsync()
{
    var oauth = new OAuthRequest
    {
        Method = "GET",
        RequestUrl = _options.OAuthGetRequestTokenUrl,
        ConsumerKey = _options.ConsumerKey,
        ConsumerSecret = _options.ConsumerSecret,
        CallbackUrl = _options.CallbackUrl
    };

    using var client = new HttpClient();
    var request = new HttpRequestMessage(HttpMethod.Get, oauth.RequestUrl);
    request.Headers.Add("Authorization", oauth.GetAuthorizationHeader());

    var response = await client.SendAsync(request);
    var responseText = await response.Content.ReadAsStringAsync();

    var query = System.Web.HttpUtility.ParseQueryString(responseText);
    var token = query["oauth_token"];
    var tokenSecret = query["oauth_token_secret"];

    _cache.StoreRequestToken(token!, tokenSecret!);

    var authorizeUrl = $"{_options.OAuthAuthorizeTokenUrl}?oauth_token={token}&name=DefectManager&scope=read,write&expiration=never";
    return authorizeUrl;
}

public async Task<bool> HandleCallbackAsync(string oauth_token, string oauth_verifier, string userId)
{
    var tokenSecret = _cache.GetRequestTokenSecret(oauth_token);

    if (tokenSecret is null)
        return false;

    var oauth = new OAuthRequest
    {
        Method = "GET",
        RequestUrl = _options.OAuthGetAccessTokenUrl,
        ConsumerKey = _options.ConsumerKey,
        ConsumerSecret = _options.ConsumerSecret,
        Token = oauth_token,
        TokenSecret = tokenSecret,
        Verifier = oauth_verifier
    };

    using var client = new HttpClient();
    var request = new HttpRequestMessage(HttpMethod.Get, oauth.RequestUrl);
    request.Headers.Add("Authorization", oauth.GetAuthorizationHeader());

    var response = await client.SendAsync(request);
    var responseText = await response.Content.ReadAsStringAsync();
    var query = System.Web.HttpUtility.ParseQueryString(responseText);

    var accessToken = query["oauth_token"];
    var accessTokenSecret = query["oauth_token_secret"];

    if (accessToken == null || accessTokenSecret == null)
        return false;

    _cache.StoreAccessToken(userId, accessToken, accessTokenSecret);

    return true;
}


        private (string token, string tokenSecret) EnsureToken(string userId)
        {
            var tokenData = _cache.GetAccessToken(userId) ?? throw new UnauthorizedAccessException("Usuário não autenticado com Trello.");
            return tokenData;
        }

        private async Task<string> TrelloGetAsync(string url, string token, string tokenSecret)
        {
            var oauth = new OAuthRequest
            {
                Method = "GET",
                RequestUrl = url,
                ConsumerKey = _options.ConsumerKey,
                ConsumerSecret = _options.ConsumerSecret,
                Token = token,
                TokenSecret = tokenSecret
            };

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", oauth.GetAuthorizationHeader());

            using var client = new HttpClient();
            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> TrelloPostAsync(string url, string token, string tokenSecret)
        {
            var oauth = new OAuthRequest
            {
                Method = "POST",
                RequestUrl = url,
                ConsumerKey = _options.ConsumerKey,
                ConsumerSecret = _options.ConsumerSecret,
                Token = token,
                TokenSecret = tokenSecret
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("Authorization", oauth.GetAuthorizationHeader());

            using var client = new HttpClient();
            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
