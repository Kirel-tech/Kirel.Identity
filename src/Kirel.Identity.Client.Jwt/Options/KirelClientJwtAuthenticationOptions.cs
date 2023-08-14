namespace Kirel.Identity.Client.Jwt.Options;

/// <summary>
/// JWT Authentication options
/// </summary>
public class KirelClientJwtAuthenticationOptions
{
    /// <summary>
    /// Base url to authentication API endpoint
    /// </summary>
    public string BaseUrl { get; set; }

    /// <summary>
    /// Relative url to authentication API endpoint
    /// </summary>
    public string RelativeUrl { get; set; }
}