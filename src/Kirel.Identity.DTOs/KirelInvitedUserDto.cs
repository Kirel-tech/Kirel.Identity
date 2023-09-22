namespace Kirel.Identity.DTOs;

/// <summary>
/// Dto for invited user
/// </summary>
/// <typeparam name="TKey">Id type</typeparam>
public class KirelInvitedUserDto<TKey> 
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey> 
{
    /// <summary>
    /// Invited user id
    /// </summary>
    public TKey Id { get; set; } = default!;
}