namespace Kirel.Identity.Server.API.Configs;

/// <summary>
/// Configuration class for email send
/// </summary>
public class EmailConfig
{
    /// <summary>
    /// Smtp host address
    /// </summary>
    public string Host { get; set; } = "";
    /// <summary>
    /// Smtp host port
    /// </summary>
    public int Port { get; set; } = 0;
    /// <summary>
    /// Login to authenticate in smtp host
    /// </summary>
    public string Login { get; set; } = "";
    /// <summary>
    /// Password to authenticate in smtp host
    /// </summary>
    public string Password { get; set; } = "";
    /// <summary>
    /// Display name associated with address
    /// </summary>
    public string? DisplayName { get; set; } = "";
    /// <summary>
    /// Value indicating whether the mail message body is in HTML
    /// </summary>
    public bool IsBodyHtml { get; set; } = false;
}