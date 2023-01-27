using System.IdentityModel.Tokens.Jwt;

namespace Kirel.Identity.Client.Jwt.Helpers;

/// <summary>
/// Provides methods for working with JWT tokens
/// </summary>
public static class KirelJwtTokenHelper
{
    /// <summary>
    /// Determines if the token has expired.
    /// </summary>
    /// <param name="jwtToken">JWT token string.</param>
    /// <returns>true - if expired.</returns>
    public static bool TokenIsExpired(string jwtToken)
    {
        var token = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
        return token.ValidTo.ToUniversalTime() < DateTime.UtcNow.AddSeconds(10);
    }
    /// <summary>
    /// Allows you to get the expiration time of the JWT token.
    /// </summary>
    /// <param name="jwtToken">JWT token string.</param>
    /// <returns>DateTimeOffset with expiration time</returns>
    public static DateTimeOffset GetExpirationTime(string jwtToken)
    {
        var token = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
        return token.ValidTo.ToUniversalTime();
    }
}