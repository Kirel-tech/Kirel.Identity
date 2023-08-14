using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Models;

/// <summary>
/// Represents a login and its associated provider for a user.
/// </summary>
/// <typeparam name="TKey"> The type of the primary key of the user associated with this login. </typeparam>
public class KirelIdentityUserLogin<TKey> : IdentityUserLogin<TKey>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
{
}