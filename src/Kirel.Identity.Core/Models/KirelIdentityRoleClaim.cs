using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Models;

/// <summary>
/// Represents a claim that is granted to all users within a role.
/// </summary>
/// <typeparam name="TKey"> The type of the primary key of the role associated with this claim. </typeparam>
public class KirelIdentityRoleClaim<TKey> : IdentityRoleClaim<TKey>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
{
}