using Kirel.Identity.Core.Models;
using Kirel.Identity.Jwt.Core.Services;
using Kirel.Identity.Server.Domain;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Server.Core.Services;

/// <inheritdoc />
public class JwtTokenService : KirelJwtTokenService<Guid, User, Role, UserRole, UserClaim, RoleClaim>
{
    /// <inheritdoc />
    public JwtTokenService(KirelAuthOptions authOptions) :
        base(authOptions)
    {
    }
}