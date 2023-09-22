
using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Server.API.Configs;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace Kirel.Identity.Server.Infrastructure.Managers;

/// <inheritdoc />
public class SmtpEmailSenderManager : IKirelEmailSender
{
    /// <summary>
    /// Email configuration
    /// </summary>
    private readonly EmailConfig _emailConfig;
    
    /// <summary>
    /// Constructor for SmtpEmailSenderManager
    /// </summary>
    /// <param name="emailConfig">Email configuration</param>
    public SmtpEmailSenderManager(EmailConfig emailConfig)
    {
        _emailConfig = emailConfig;
    }
    
    /// <summary>
    /// Sends email message async
    /// </summary>
    /// <param name="subject">Email message subject</param>
    /// <param name="body">Email message body</param>
    /// <param name="emailAddress">Email address to send to</param>
    public async Task SendEmailAsync(string subject, string body, string emailAddress)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(
            "CitMed",
            _emailConfig.Login
        ));
            
        emailMessage.To.Add(new MailboxAddress("", emailAddress));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(TextFormat.Html)
        {
            Text = body
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_emailConfig.Host, _emailConfig.Port, true);
        await client.AuthenticateAsync(_emailConfig.Login, _emailConfig.Password);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }
}