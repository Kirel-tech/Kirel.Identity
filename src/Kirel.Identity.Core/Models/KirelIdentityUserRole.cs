using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Models;

/// <summary>
/// Represents the link between a user and a role.
/// </summary>
/// <typeparam name="TKey"> The type of the primary key used for users and roles. </typeparam>
/// <typeparam name="TUserRole"> The user role entity type. </typeparam>
/// <typeparam name="TUser"> The user entity type. </typeparam>
/// <typeparam name="TRole"> The role entity type. </typeparam>
/// <typeparam name="TUserClaim"> User claim type. </typeparam>
/// <typeparam name="TRoleClaim"> Role claim type. </typeparam>
public class KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim> : IdentityUserRole<TKey>
where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
where TRoleClaim : IdentityRoleClaim<TKey>
where TUserClaim : IdentityUserClaim<TKey>
{
    /// <summary>
    /// Linked user entity
    /// </summary>
    public virtual TUser User { get; set; } = null!;
    /// <summary>
    /// Linked role entity
    /// </summary>
    public virtual TRole Role { get; set; } = null!;
}