using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Services;
using Kirel.Identity.Server.Domain;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Server.Core.Services;

/// <inheritdoc />
public class SmsAuthenticationService : KirelSmsAuthenticationService<Guid, User, Role, UserRole, UserClaim, RoleClaim>
{
    /// <inheritdoc />
    public SmsAuthenticationService(UserManager<User> userManager, IKirelSmsSender kirelSmsSender) : base(userManager, kirelSmsSender)
    {
    }
}