using Example.API.Models;
using Kirel.Identity.Core.Context;
using Microsoft.EntityFrameworkCore;

namespace Example.API.Contexts;

/// <inheritdoc />
public class ExKirelIdentityContext : KirelIdentityContext<ExUser, ExRole, Guid, ExUserClaim, ExUserRole, ExUserLogin,
    ExRoleClaim, ExUserToken>
{
    /// <inheritdoc />
    public ExKirelIdentityContext(DbContextOptions options) : base(options)
    {
    }
}