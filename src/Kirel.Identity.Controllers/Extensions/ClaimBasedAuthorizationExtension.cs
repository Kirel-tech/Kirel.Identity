using Microsoft.Extensions.DependencyInjection;

namespace Kirel.Identity.Controllers.Extensions;

/// <summary>
/// Extension methods for setting up claim based authorization in an <see cref="IServiceCollection" />.
/// </summary>
public static class ClaimBasedAuthorizationExtension
{
    /// <summary>
    /// Adds authorization services to the specified <see cref="IServiceCollection" />. 
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static void AddClaimBasedAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(opts => {
            opts.AddPolicy(
            "user_create", b => b.RequireClaim("user", "create")
            );
            opts.AddPolicy(
            "user_read",b =>  b.RequireClaim("user", "read")
            );
            opts.AddPolicy(
            "user_update",b =>  b.RequireClaim("user", "update")
            );
            opts.AddPolicy(
            "user_delete",b =>  b.RequireClaim("user", "delete")
            );
            opts.AddPolicy(
            "role_create", b => b.RequireClaim("role_claim", "create")
            );
            opts.AddPolicy(
            "role_read",b =>  b.RequireClaim("role_claim", "read")
            );
            opts.AddPolicy(
            "role_update",b =>  b.RequireClaim("role_claim", "update")
            );
            opts.AddPolicy(
            "role_delete",b =>  b.RequireClaim("role_claim", "delete")
            );
        });
    }
}