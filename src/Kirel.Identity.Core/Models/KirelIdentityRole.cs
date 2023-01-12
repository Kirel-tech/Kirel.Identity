using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Models;

/// <summary>
/// The default implementation of <see cref="IdentityRole"/> which uses a string as the primary key.
/// </summary>
public class KirelIdentityRole<TKey> : IdentityRole<TKey> where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
{
}
