using Microsoft.Extensions.Caching.Memory;

namespace GestaoDefeitos.WebApi.Cache
{
    public class TrelloRequestTokenCach(IMemoryCache _cache) : ITrelloRequestTokenCache
    {
        public void StoreAccessToken(string userId, string token, string tokenSecret)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };

            _cache.Set($"access:{userId}", (token, tokenSecret), options);
        }

        public (string token, string tokenSecret)? GetAccessToken(string userId)
        {
            return _cache.TryGetValue($"access:{userId}", out (string, string) result) ? result : null;
        }

        public void StoreRequestToken(string requestToken, string tokenSecret)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            _cache.Set($"request:{requestToken}", tokenSecret, options);
        }

        public string? GetRequestTokenSecret(string requestToken)
        {
            return _cache.TryGetValue($"request:{requestToken}", out string? result) ? result : null;
        }
    }
}
