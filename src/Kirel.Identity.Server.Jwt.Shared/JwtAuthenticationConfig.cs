namespace Kirel.Identity.Server.Jwt.Shared;

/// <summary>
/// JWT Authentication parameters
/// </summary>
public class JwtAuthenticationConfig
{
    /// <summary>
    /// Token issuer name
    /// </summary>
    public string Issuer { get; set; } = "";

    /// <summary>
    /// Token audience name
    /// </summary>
    public string Audience { get; set; } = "";

    /// <summary>
    /// Token secret key
    /// </summary>
    public string Key { get; set; } = "";

    /// <summary>
    /// Token lifetime in minutes
    /// </summary>
    public int AccessLifetime { get; set; } = 20;

    /// <summary>
    /// Refresh token lifetime in minutes
    /// </summary>
    public int RefreshLifetime { get; set; } = 3 * 24 * 60;
}