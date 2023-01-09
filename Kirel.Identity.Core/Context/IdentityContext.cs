using Kirel.Identity.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kirel.Identity.Core.Context;

/// <summary>
/// Class for the Identity Framework database context used for Guid identity.
/// </summary>
public class IdentityContext<TKey, TUser> : IdentityDbContext<TUser, IdentityRole<TKey>, TKey, 
    IdentityUserClaim<TKey>,IdentityUserRole<TKey>,IdentityUserLogin<TKey>,IdentityRoleClaim<TKey>,IdentityUserToken<TKey>> 
    where TUser : IdentityUser<TKey>, IKirelUser<TKey> 
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
{
    /// <summary>
    /// Identity context constructor
    /// </summary>
    /// <param name="options">is DbContextOptions</param>
    public IdentityContext(DbContextOptions options) : base(options)
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

