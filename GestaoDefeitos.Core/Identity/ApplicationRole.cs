using Microsoft.AspNetCore.Identity;

namespace GestaoDefeitos.Domain.AspNetIdentity
{
    public class ApplicationRole : IdentityRole<Guid> { }

    public class ApplicationUserClaim : IdentityUserClaim<Guid> { }

    public class ApplicationUserLogin : IdentityUserLogin<Guid> { }

    public class ApplicationUserToken : IdentityUserToken<Guid> { }

    public class ApplicationRoleClaim : IdentityRoleClaim<Guid> { }

    public class ApplicationUserRole : IdentityUserRole<Guid> { }

}
