using Kirel.Identity.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Models;

/// <summary>
/// The default implementation of <see cref="IdentityUser{TKey}"/> which uses a string as a primary key.
/// </summary>
public class KirelIdentityUser<TKey> : IdentityUser<TKey>, IKirelUser<TKey> where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
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
}
