namespace Kirel.Identity.DTOs;

/// <summary>
/// Jwt token dto class 
/// </summary>
public class JwtTokenDto
{
    /// <summary>
    /// Access jwt token
    /// </summary>
    public string AccessToken { get; set; } = "";
    /// <summary>
    /// Refresh jwt token
    /// </summary>
    public string RefreshToken { get; set; } = "";
}