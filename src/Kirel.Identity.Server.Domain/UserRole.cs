using Kirel.Identity.Core.Models;

namespace Kirel.Identity.Server.Domain;

/// <inheritdoc />
public class UserRole : KirelIdentityUserRole<Guid, UserRole, User, Role, UserClaim, RoleClaim>
{
}