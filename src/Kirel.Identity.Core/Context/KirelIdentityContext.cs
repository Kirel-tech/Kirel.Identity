using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kirel.Identity.Core.Context;

/// <summary>
/// Kirel base class for the Identity Framework database context.
/// </summary>
public class KirelIdentityContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin,
        TRoleClaim, TUserToken>
    : IdentityDbContext<TUser, TRole, TKey,
        TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
    where TUserClaim : KirelIdentityUserClaim<TKey>
    where TUserLogin : KirelIdentityUserLogin<TKey>
    where TRoleClaim : KirelIdentityRoleClaim<TKey>
    where TUserToken : KirelIdentityUserToken<TKey>
{
    /// <summary>
    /// Kirel base class for the Identity Framework database context constructor
    /// </summary>
    /// <param name="options"> is DbContextOptions </param>
    public KirelIdentityContext(DbContextOptions options) : base(options)
    {
    }

    /// <summary>
    /// Method which called when entity saved
    /// </summary>
    /// <param name="cancellationToken"> token </param>
    /// <returns>
    /// A task that represents the asynchronous save operation.
    /// The task result contains the number of state entries written to the database
    /// </returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        DateTracking();
        return base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Method which called when entity saved
    /// </summary>
    /// <returns> The number of state entries written to the database </returns>
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

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)  
    {  
        base.OnModelCreating(modelBuilder);  
        // User Roles relationships
        modelBuilder.Entity<TUserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });
        modelBuilder.Entity<TUserRole>()
            .HasOne(mg => mg.User)
            .WithMany(m => m.UserRoles)
            .HasForeignKey(mg => mg.UserId);
        modelBuilder.Entity<TUserRole>()
            .HasOne(mg => mg.Role)
            .WithMany(g => g.UserRoles)
            .HasForeignKey(mg => mg.RoleId);
        
        //Include Role automatically for UserRole
        modelBuilder.Entity<TUserRole>()
            .Navigation(e => e.Role)
            .AutoInclude();
        //Include Claims automatically for User
        modelBuilder.Entity<TUser>()
            .Navigation(e => e.Claims)
            .AutoInclude();
        //Include UserRoles automatically for User
        modelBuilder.Entity<TUser>()
            .Navigation(e => e.UserRoles)
            .AutoInclude();
        //Include Claims automatically for Role
        modelBuilder.Entity<TRole>()
            .Navigation(e => e.Claims)
            .AutoInclude();
        
        
        
        // User claims relationships
        modelBuilder.Entity<TUser>()
            .HasMany(e => e.Claims)
            .WithOne()
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Role claims relationships
        modelBuilder.Entity<TRole>().HasMany(e => e.Claims)
            .WithOne()
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}