using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Models;

/// <summary>
/// Represents a claim that a user possesses.
/// </summary>
/// <typeparam name="TKey"> The type used for the primary key for this user that possesses this claim. </typeparam>
public class KirelIdentityUserClaim<TKey> : IdentityUserClaim<TKey>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
{
}