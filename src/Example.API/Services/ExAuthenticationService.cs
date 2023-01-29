using Example.API.Models;
using Kirel.Identity.Core.Services;
using Microsoft.AspNetCore.Identity;

namespace Example.API.Services;

/// <inheritdoc />
public class ExAuthenticationService : KirelAuthenticationService<Guid, ExUser>
{
    /// <inheritdoc />
    public ExAuthenticationService(UserManager<ExUser> userManager) : base(userManager)
    {
    }
}