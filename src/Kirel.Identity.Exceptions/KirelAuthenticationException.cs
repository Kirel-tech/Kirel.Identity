namespace Kirel.Identity.Exceptions;

/// <summary>
/// The exception that is thrown when authentication was failed
/// </summary>
public class KirelAuthenticationException : Exception
{
    /// <summary>
    /// Initializes a new instance of Kirel authentication exception.
    /// </summary>
    public KirelAuthenticationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of Kirel authentication exception.
    /// </summary>
    /// <param name="message"> Exception message </param>
    public KirelAuthenticationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of Kirel authentication exception.
    /// </summary>
    /// <param name="message"> Exception message </param>
    /// <param name="innerEx"> Inner exception </param>
    public KirelAuthenticationException(string message, Exception innerEx) : base(message, innerEx)
    {
    }
}