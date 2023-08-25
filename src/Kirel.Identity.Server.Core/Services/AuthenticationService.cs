using Kirel.Identity.Core.Services;
using Kirel.Identity.Server.Domain;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Server.Core.Services;

/// <inheritdoc />
public class AuthenticationService : KirelAuthenticationService<Guid, User, Role, UserRole>
{
    /// <inheritdoc />
    public AuthenticationService(UserManager<User> userManager) : base(userManager)
    {
    }
}