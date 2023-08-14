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
    /// <param name="jwtToken"> JWT token string. </param>
    /// <returns> true - if expired. </returns>
    public static bool TokenIsExpired(string jwtToken)
    {
        var token = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
        return token.ValidTo.ToUniversalTime() < DateTime.UtcNow;
    }

    /// <summary>
    /// Allows you to get the expiration time of the JWT token.
    /// </summary>
    /// <param name="jwtToken"> JWT token string. </param>
    /// <returns> <see cref="DateTime" /> with expiration time in UTC </returns>
    public static DateTime GetExpirationTime(string jwtToken)
    {
        var token = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
        return token.ValidTo.ToUniversalTime();
    }
}