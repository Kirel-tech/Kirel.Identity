namespace Kirel.Identity.Exceptions;

/// <summary>
/// The exception that is thrown when the entity was not found
/// </summary>
[Serializable]
public class KirelNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of Kirel not found exception.
    /// </summary>
    public KirelNotFoundException()
    {
    }

    /// <summary>
    /// Initializes a new instance of Kirel not found exception.
    /// </summary>
    /// <param name="message"> Exception message </param>
    public KirelNotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of Kirel not found exception.
    /// </summary>
    /// <param name="message"> Exception message </param>
    /// <param name="innerEx"> Inner exception </param>
    public KirelNotFoundException(string message, Exception innerEx) : base(message, innerEx)
    {
    }
}