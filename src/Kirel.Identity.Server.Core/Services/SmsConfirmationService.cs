using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Services;
using Kirel.Identity.Server.Domain;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Server.Core.Services;

/// <inheritdoc />
public class SmsConfirmationService : KirelSmsConfirmationService<Guid, User, Role, UserRole, UserClaim, RoleClaim>
{
    /// <inheritdoc />
    public SmsConfirmationService(UserManager<User> userManager, IKirelSmsSender kirelSmsSender) : base(userManager, kirelSmsSender)
    {
    }
}