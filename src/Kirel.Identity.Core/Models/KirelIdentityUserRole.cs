using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Models;

/// <summary>
/// Represents the link between a user and a role.
/// </summary>
/// <typeparam name="TKey"> The type of the primary key used for users and roles. </typeparam>
public class KirelIdentityUserRole<TKey> : IdentityUserRole<TKey>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
{
}