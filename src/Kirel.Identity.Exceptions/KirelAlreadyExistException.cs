namespace Kirel.Identity.Exceptions;

/// <summary>
/// The exception that is thrown when the entity already exist
/// </summary>
[Serializable]
public class KirelAlreadyExistException : Exception
{
    /// <summary>
    /// Initializes a new instance of Kirel already exist exception.
    /// </summary>
    public KirelAlreadyExistException(){}
    /// <summary>
    /// Initializes a new instance of Kirel already exist exception.
    /// </summary>
    /// <param name="message">Exception message</param>
    public KirelAlreadyExistException(string message): base(message) {}
    /// <summary>
    /// Initializes a new instance of Kirel already exist exception.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="innerEx">Inner exception</param>
    public KirelAlreadyExistException(string message, Exception innerEx) : base(message, innerEx) {}
}