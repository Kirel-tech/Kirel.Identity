using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Models;

/// <summary>
/// The default implementation of <see cref="IdentityRole" /> which uses a string as the primary key.
/// </summary>
public class KirelIdentityRole<TKey, TRole, TUser, TUserRole> : IdentityRole<TKey> 
where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole>
where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole>
where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole>
{
    /// <summary>
    /// List of user roles 
    /// </summary>
    public virtual ICollection<TUserRole> UserRoles { get; set; } = null!;
}