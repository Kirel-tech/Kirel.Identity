using System.Net.Mail;

namespace Kirel.Identity.Core.Interfaces;

/// <summary>
/// Represents an interface for sending email messages asynchronously.
/// </summary>
public interface IMailSender
{
    /// <summary>
    /// Sends an email message asynchronously.
    /// </summary>
    /// <param name="recipient"> The recipient's email address. </param>
    /// <param name="message"> The email message to send. </param>
    /// <returns> A task representing the asynchronous operation. </returns>
    Task SendAsync(string recipient, MailMessage message);
}