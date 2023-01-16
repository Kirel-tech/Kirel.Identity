using Example.Models;
using Kirel.Identity.Core.Models;
using Kirel.Identity.Jwt.Core.Services;
using Microsoft.AspNetCore.Identity;

namespace Example.Services;

/// <inheritdoc />
public class ExJwtTokenService : KirelJwtTokenService<Guid, ExUser, ExRole>
{
    /// <inheritdoc />
    public ExJwtTokenService(UserManager<ExUser> userManager, RoleManager<ExRole> roleManager, KirelAuthOptions authOptions) : base(userManager, roleManager, authOptions)
    {
    }
}