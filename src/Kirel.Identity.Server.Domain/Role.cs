using Kirel.Identity.Core.Models;

namespace Kirel.Identity.Server.Domain;

/// <inheritdoc />
public class Role : KirelIdentityRole<Guid, Role, User, UserRole>
{
}