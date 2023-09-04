using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Services;
using Kirel.Identity.Server.Domain;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Server.Core.Services;

/// <inheritdoc />
public class EmailConfirmationService : KirelEmailConfirmationService<Guid, User, Role, UserRole>
{
    /// <inheritdoc />
    public EmailConfirmationService(UserManager<User> userManager, IMailSender mailSender) : base(userManager,
        mailSender)
    {
    }
}