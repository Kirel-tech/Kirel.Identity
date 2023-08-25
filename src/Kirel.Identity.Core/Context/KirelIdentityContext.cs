﻿using Kirel.Identity.Core.Interfaces;
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
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUserClaim : KirelIdentityUserClaim<TKey>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole>
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
        modelBuilder.Entity<TUser>(b =>  
        {     
            // Each User can have many entries in the UserRole join table  
            b.HasMany(e => e.UserRoles)  
                .WithOne(e => e.User)  
                .HasForeignKey(ur => ur.UserId)  
                .IsRequired();  
        });  

        modelBuilder.Entity<TRole>(b =>  
        {  
            // Each Role can have many entries in the UserRole join table  
            b.HasMany(e => e.UserRoles)  
                .WithOne(e => e.Role)  
                .HasForeignKey(ur => ur.RoleId)  
                .IsRequired();  
        });  
    }
}