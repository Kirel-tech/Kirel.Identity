namespace Kirel.Identity.Core.Models;

/// <summary>
/// Options for invites
/// </summary>
public class KirelInvitesOptions
{
    /// <summary>
    /// This is a url that leads to the point where the user continues his registration
    /// </summary>
    public string ContinueRegistrationUrl { get; set; } = "";
}