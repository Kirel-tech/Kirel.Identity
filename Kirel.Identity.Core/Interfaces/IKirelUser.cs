namespace Kirel.Identity.Core.Interfaces;

/// <summary>
/// The interface that the user class must implement in the main part of the application
/// </summary>
/// <typeparam name="TKey">user id type like int, string, guid etc.</typeparam>
public interface IKirelUser<TKey> where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
{
    /// <summary>
    /// User id
    /// </summary>
    public TKey Id { get; set; }
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