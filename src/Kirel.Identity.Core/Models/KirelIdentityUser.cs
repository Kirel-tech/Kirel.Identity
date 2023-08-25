using Kirel.Identity.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Models;

/// <summary>
/// The default implementation of <see cref="IdentityUser{TKey}" />.
/// </summary>
public class KirelIdentityUser<TKey, TUser, TRole, TUserRole> : IdentityUser<TKey>, IKirelUser<TKey>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole>
{
    /// <summary>
    /// First name of the user
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Last name of the user
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// User create date and time
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// User last update date and time
    /// </summary>
    public DateTime? Updated { get; set; }

    /// <summary>
    /// List of user roles
    /// </summary>
    public virtual ICollection<TUserRole> UserRoles { get; set; } = null!;
}