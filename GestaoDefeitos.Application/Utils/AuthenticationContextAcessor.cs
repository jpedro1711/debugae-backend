using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GestaoDefeitos.Application.Utils
{
    public class AuthenticationContextAcessor(IHttpContextAccessor _httpContextAccessor)
    {
        public Guid GetCurrentLoggedUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out var userId))
                return userId;

            throw new InvalidOperationException("User ID not found in the current context or is not a valid GUID.");
        }

        public string GetCurrentLoggedUserName()
        {
            var userName = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
            if (userName is not null)
                return userName;

            throw new InvalidOperationException("User name not found in the current context.");
        }
    }
}
