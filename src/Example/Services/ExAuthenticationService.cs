using Example.Models;
using Kirel.Identity.Core.Services;
using Microsoft.AspNetCore.Identity;

namespace Example.Services;

/// <inheritdoc />
public class ExAuthenticationService : KirelAuthenticationService<Guid, ExUser>
{
    /// <inheritdoc />
    public ExAuthenticationService(UserManager<ExUser> userManager) : base(userManager)
    {
    }
}