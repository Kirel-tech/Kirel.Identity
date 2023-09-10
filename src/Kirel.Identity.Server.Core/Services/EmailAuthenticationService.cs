using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Models;
using Kirel.Identity.Core.Services;
using Kirel.Identity.Server.Domain;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Server.Core.Services;

/// <inheritdoc />
public class EmailAuthenticationService : KirelEmailAuthenticationService<Guid, User, Role, UserRole>
{
    /// <inheritdoc />
    public EmailAuthenticationService(UserManager<User> userManager, IMailSender mailSender,
        KirelUserToken<Guid> kirelUserToken) : base(userManager, mailSender, kirelUserToken)
    {
    }
}