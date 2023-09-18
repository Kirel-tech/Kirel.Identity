namespace Kirel.Identity.Server.Infrastructure.Models;

/// <summary>
/// Config for main sms sender
/// </summary>
public class MainSmsSenderConfig
{
    /// <summary>
    /// Api key for connection with main sms service
    /// </summary>
    public string ApiKey { get; set; } = "";
    /// <summary>
    /// Main sms project name
    /// </summary>
    public string Project { get; set; } = "";
}