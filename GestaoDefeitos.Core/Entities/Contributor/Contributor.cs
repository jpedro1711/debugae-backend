using Microsoft.AspNetCore.Identity;

namespace GestaoDefeitos.Domain.Entities.Contributor
{
    public class Contributor : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
