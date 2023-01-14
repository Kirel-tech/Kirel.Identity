using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kirel.Identity.Core.Context;

/// <summary>
/// Kirel base class for the Identity Framework database context.
/// </summary>
public class KirelIdentityContext<TUser, TRole, TKey, TIdentityUserClaim, TIdentityUserRole, TIdentityUserLogin, 
    TIdentityRoleClaim, TIdentityUserToken> 
    : IdentityDbContext<TUser, TRole, TKey, 
        TIdentityUserClaim,TIdentityUserRole,TIdentityUserLogin,TIdentityRoleClaim,TIdentityUserToken> 
    where TUser : KirelIdentityUser<TKey>
    where TRole : KirelIdentityRole<TKey>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TIdentityUserClaim : KirelIdentityUserClaim<TKey>
    where TIdentityUserRole : KirelIdentityUserRole<TKey>
    where TIdentityUserLogin : KirelIdentityUserLogin<TKey>
    where TIdentityRoleClaim : KirelIdentityRoleClaim<TKey>
    where TIdentityUserToken : KirelIdentityUserToken<TKey>
{
    /// <summary>
    /// Kirel base class for the Identity Framework database context constructor
    /// </summary>
    /// <param name="options">is DbContextOptions</param>
    public KirelIdentityContext(DbContextOptions options) : base(options)
    {
    }
    
    /// <summary>
    /// Method which called when entity saved
    /// </summary>
    /// <param name="cancellationToken">token</param>
    /// <returns>A task that represents the asynchronous save operation.
    /// The task result contains the number of state entries written to the database</returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        DateTracking();
        return base.SaveChangesAsync(cancellationToken);
    }
    /// <summary>
    /// Method which called when entity saved
    /// </summary>
    /// <returns>The number of state entries written to the database</returns>
    public override int SaveChanges()
    {
        DateTracking();
        return base.SaveChanges();
    }

    /// <summary>
    /// Method for setting the desired date format
    /// </summary>
    private void DateTracking()
    {
        foreach (var entry in ChangeTracker.Entries<IKirelUser<TKey>>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.Updated = DateTime.UtcNow;
                    break;
            }
        }
    }
}

