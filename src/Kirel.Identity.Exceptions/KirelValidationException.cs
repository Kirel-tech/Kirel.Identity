namespace Kirel.Identity.Exceptions;

/// <summary>
/// The exception that is thrown when validation was failed
/// </summary>
[Serializable]
public class KirelValidationException : Exception
{
    /// <summary>
    /// Initializes a new instance of Kirel validation exception.
    /// </summary>
    public KirelValidationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of Kirel validation exception.
    /// </summary>
    /// <param name="message"> Exception message </param>
    public KirelValidationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of Kirel validation exception.
    /// </summary>
    /// <param name="message"> Exception message </param>
    /// <param name="innerEx"> Inner exception </param>
    public KirelValidationException(string message, Exception innerEx) : base(message, innerEx)
    {
    }
}