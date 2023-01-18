namespace Kirel.Identity.Exceptions;

/// <summary>
/// Represents errors that occurs when user or role managers fails on store based operations like update, create etc.
/// </summary>
[Serializable]
public class KirelIdentityStoreException : Exception
{   
    /// <summary>
    /// Initializes a new instance of Kirel identity store exception.
    /// </summary>
    public KirelIdentityStoreException(){}
    /// <summary>
    /// Initializes a new instance of Kirel identity store exception.
    /// </summary>
    /// <param name="message">Exception message</param>
    public KirelIdentityStoreException(string message): base(message) {}
    /// <summary>
    /// Initializes a new instance of Kirel identity store exception.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="innerEx">Inner exception</param>
    public KirelIdentityStoreException(string message, Exception innerEx) : base(message, innerEx) {}
}