using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Kirel.Identity.Core.Models;

/// <summary>
/// Jwt token authentication options class
/// </summary>
public class AuthOptions
{
    /// <summary>
    /// AuthOptions constructor
    /// </summary>
    /// <param name="issuer">Token publisher</param>
    /// <param name="audience">Token consumer</param>
    /// <param name="key">Encryption key</param>
    /// <param name="accessLifetime">Access token lifetime(in minutes)</param>
    /// <param name="refreshLifetime">Refresh token lifetime(in minutes)</param>
    public AuthOptions(string issuer, string audience, string key, int accessLifetime, int refreshLifetime)
    {
        Issuer = issuer;
        Audience = audience;
        Key = key;
        AccessLifetime = accessLifetime;
        RefreshLifetime = refreshLifetime;
    }

    /// <summary>
    /// AuthOptions constructor
    /// </summary>
    public AuthOptions()
    {
    }

    /// <summary>
    /// Token publisher
    /// </summary>
    public string Issuer { get; set; } = "";
    /// <summary>
    /// Token consumer
    /// </summary>
    public string Audience { get; set; } = "";
    /// <summary>
    /// Encryption key
    /// </summary>
    public string Key { get; set; } = "";
    /// <summary>
    /// Access token lifetime(in minutes)
    /// </summary>
    public int AccessLifetime { get; set; }
    /// <summary>
    /// Refresh token lifetime(in minutes)
    /// </summary>
    public int RefreshLifetime { get; set; }

    /// <summary>
    /// Method for getting symmetric security key
    /// </summary>
    /// <returns>Symmetric security key</returns>
    public SymmetricSecurityKey GetSymmetricSecurityKey(string key)
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
    }
}