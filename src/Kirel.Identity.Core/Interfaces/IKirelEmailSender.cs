namespace Kirel.Identity.Core.Interfaces;

/// <summary>
/// Interface for email sender
/// </summary>
public interface IKirelEmailSender
{
    /// <summary>
    /// Sends email message to the given email address
    /// </summary>
    /// <returns></returns>
    Task SendEmailAsync(string subject, string body, string emailAddress);
}