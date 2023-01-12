using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Models;

/// <summary>
/// Represents an authentication token for a user.
/// </summary>
/// <typeparam name="TKey">The type of the primary key used for users.</typeparam>
public class KirelIdentityUserToken<TKey> : IdentityUserToken<TKey> where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
{
   
}