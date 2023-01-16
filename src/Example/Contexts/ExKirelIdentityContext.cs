using Example.Models;
using Kirel.Identity.Core.Context;
using Microsoft.EntityFrameworkCore;

namespace Example.Contexts;

/// <inheritdoc />
public class ExKirelIdentityContext : KirelIdentityContext<ExUser, ExRole, Guid, ExUserClaim, ExUserRole, ExUserLogin,
    ExRoleClaim, ExUserToken>
{
    /// <inheritdoc />
    public ExKirelIdentityContext(DbContextOptions options) : base(options)
    {
    }
}