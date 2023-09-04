namespace Kirel.Identity.Core.Options;

/// <summary>
/// Represents the email settings required for SMTP configuration.
/// </summary>
public class EmailSettings
{
    /// <summary>
    /// Gets or sets the SMTP server address.
    /// </summary>
    public string SmtpServer { get; set; } = "";

    /// <summary>
    /// Gets or sets the SMTP port number.
    /// </summary>
    public int SmtpPort { get; set; }

    /// <summary>
    /// Gets or sets the username for SMTP authentication.
    /// </summary>
    public string SmtpUsername { get; set; } = "";

    /// <summary>
    /// Gets or sets the password for SMTP authentication.
    /// </summary>
    public string SmtpPassword { get; set; } = "";

    /// <summary>
    /// Gets or sets the email address used as the sender's address for SMTP emails.
    /// </summary>
    public string SmtpEmail { get; set; } = "";
}