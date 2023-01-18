namespace Kirel.Identity.Exceptions;

/// <summary>
/// The exception that is thrown when user was not authorized
/// </summary>
[Serializable]
public class KirelUnauthorizedException : Exception
{
    /// <summary>
    /// Initializes a new instance of Kirel unauthorized exception.
    /// </summary>
    public KirelUnauthorizedException(){}
    /// <summary>
    /// Initializes a new instance of Kirel unauthorized exception.
    /// </summary>
    /// <param name="message">Exception message</param>
    public KirelUnauthorizedException(string message): base(message) {}
    /// <summary>
    /// Initializes a new instance of Kirel unauthorized exception.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="innerEx">Inner exception</param>
    public KirelUnauthorizedException(string message, Exception innerEx) : base(message, innerEx) {}
}