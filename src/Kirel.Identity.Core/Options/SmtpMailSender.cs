using System.Net;
using System.Net.Mail;
using Kirel.Identity.Core.Interfaces;

namespace Kirel.Identity.Core.Options;

/// <summary>
/// Implementation of the IMailSender interface that sends emails using SMTP.
/// </summary>
public class SmtpMailSender : IMailSender
{
    /// <summary>
    /// options for SMTP client
    /// </summary>
    protected readonly EmailSettings EmailSettings;

    /// <summary>
    /// Initializes a new instance of the SmtpMailSender class with the specified EmailSettings.
    /// </summary>
    /// <param name="emailSettings"> The email settings to use for sending emails. </param>
    public SmtpMailSender(EmailSettings emailSettings)
    {
        EmailSettings = emailSettings;
    }

    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="recipient"> The recipient's email address. </param>
    /// <param name="message"> The MailMessage object containing the email content. </param>
    /// <returns> A Task representing the asynchronous operation. </returns>
    public async Task SendAsync(string recipient, MailMessage message)
    {
        using (var client = new SmtpClient(EmailSettings.SmtpServer))
        {
            client.Port = EmailSettings.SmtpPort;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(EmailSettings.SmtpUsername, EmailSettings.SmtpPassword);

            message.From = new MailAddress(EmailSettings.SmtpEmail);
            message.To.Add(recipient);

            await client.SendMailAsync(message);
        }
    }
}