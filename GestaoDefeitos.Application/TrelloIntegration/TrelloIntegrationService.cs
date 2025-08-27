using GestaoDefeitos.Application.Cache;
using GestaoDefeitos.Application.TrelloIntegration.Responses;
using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Options;
using OAuth;
using System.Text.Json;

namespace GestaoDefeitos.Application.TrelloIntegration
{
    public class TrelloIntegrationService
        (
            ITrelloRequestTokenCache cache,
            IOptions<TrelloApiOptions> options,
            ITrelloUserStoryRepository _trelloUserStoryRepository
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

        public async Task<List<TrelloWorkspaceViewModel>> GetWorkspacesAsync(string userId)
        {
            var (token, secret) = EnsureToken(userId);
            var result =  await TrelloGetAsync(_options.GetWorkspacesUrl, token, secret);
            List<TrelloWorkspaceViewModel> workspaces = JsonSerializer.Deserialize<List<TrelloWorkspaceViewModel>>(result) ?? [];
            return workspaces;
        }

        public async Task<List<TrelloBoardViewModel>> GetBoardsAsync(string userId, string workspaceId)
        {
            var (token, secret) = EnsureToken(userId);
            var url = string.Format(_options.GetBoardsUrl, workspaceId);
            var result = await TrelloGetAsync(url, token, secret);
            List<TrelloBoardViewModel> boards = JsonSerializer.Deserialize<List<TrelloBoardViewModel>>(result) ?? [];
            return boards;
        }

        public async Task<List<TrelloCardViewModel>> GetCardsAsync(string userId, string boardId)
        {
            var (token, secret) = EnsureToken(userId);
            var url = string.Format(_options.GetCardsUrl, boardId);
            var result = await TrelloGetAsync(url, token, secret);
            List<TrelloCardViewModel> cards = JsonSerializer.Deserialize<List<TrelloCardViewModel>>(result) ?? [];
            return cards;
        }

        public async Task<string> GetCardDetailsAsync(string userId, string cardId)
        {
            var (token, secret) = EnsureToken(userId);
            var url = string.Format(_options.GetCardUrl, cardId);
            return await TrelloGetAsync(url, token, secret);
        }

        public async Task<TrelloUserStory> AddCommentAsync(string userId, string cardId, string comment, Guid defectId)
        {
            var (token, secret) = EnsureToken(userId);
            var url = string.Format(_options.AddCommentUrl, cardId, Uri.EscapeDataString(comment));
            await TrelloPostAsync(url, token, secret);

            var cardDetails = await GetCardDetailsAsync(userId, cardId);

            var trelloUserStoryViewModel = JsonSerializer.Deserialize<TrelloUserStoryViewModel>(cardDetails);

            TrelloUserStory trelloUserStory = new TrelloUserStory
            {
                Id = Guid.NewGuid(),
                Desc = trelloUserStoryViewModel?.Description,
                ShortUrl = trelloUserStoryViewModel?.Url,
                Name = trelloUserStoryViewModel?.Name,
                DefectId = defectId
            };

            var userStory = await _trelloUserStoryRepository.AddAsync(trelloUserStory);

            return userStory;
        }

        public async Task<string> GetLoginRedirectUrlAsync(string returnUrl)
        {
            var oauth = new OAuthRequest
            {
                Method = "GET",
                RequestUrl = _options.OAuthGetRequestTokenUrl,
                ConsumerKey = _options.ConsumerKey,
                ConsumerSecret = _options.ConsumerSecret,
                CallbackUrl = $"{_options.CallbackUrl}?returnUrl={Uri.EscapeDataString(returnUrl)}"
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

            var authorizeUrl = $"{_options.OAuthAuthorizeTokenUrl}?oauth_token={token}&name=DefectManager&scope=read,write&expiration=never&returnUrl={returnUrl}";
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
