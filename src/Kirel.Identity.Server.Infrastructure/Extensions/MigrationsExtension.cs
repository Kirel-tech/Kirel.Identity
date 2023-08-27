using Kirel.Identity.Server.Infrastructure.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kirel.Identity.Server.Infrastructure.Extensions;

/// <summary>
/// Identity db migrations Application Builder extension
/// </summary>
public static class MigrationsExtension
{
    /// <summary>
    /// Apply identity db context migration
    /// </summary>
    /// <param name="app"> Application builder </param>
    public static async Task MigrateIdentityDbAsync(this IApplicationBuilder app)
    {
        var scopedServiceProvider = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope()
            .ServiceProvider;
        var dbContext = scopedServiceProvider.GetRequiredService<IdentityServerDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}