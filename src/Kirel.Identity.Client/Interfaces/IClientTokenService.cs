namespace Kirel.Identity.Client.Interfaces;

/// <summary>
/// Describes methods for work with tokens
/// </summary>
public interface IClientTokenService
{
    /// <summary>
    /// Method for getting access token
    /// </summary>
    /// <returns>Access token</returns>
    Task<string> GetAccessTokenAsync();
    /// <summary>
    /// Method for getting refresh token
    /// </summary>
    /// <returns>Refresh token</returns>
    Task<string> GetRefreshTokenAsync();
    /// <summary>
    /// Method for setting access token
    /// </summary>
    Task SetAccessTokenAsync(string token);
    /// <summary>
    /// Method for setting refresh token
    /// </summary>
    Task SetRefreshTokenAsync(string token);
}