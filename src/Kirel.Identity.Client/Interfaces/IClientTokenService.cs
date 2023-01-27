namespace Kirel.Identity.Client.Interfaces;

public interface IClientTokenService
{
    /// <summary>
    /// Method for getting JWT access token string
    /// </summary>
    /// <returns>JWT access token</returns>
    Task<string> GetAccessTokenAsync();
    /// <summary>
    /// Method for getting JWT refresh token string
    /// </summary>
    /// <returns>JWT refresh token</returns>
    Task<string> GetRefreshTokenAsync();
    /// <summary>
    /// Method for setting JWT access token string
    /// </summary>
    Task SetAccessTokenAsync(string token);
    /// <summary>
    /// Method for setting JWT refresh token string
    /// </summary>
    Task SetRefreshTokenAsync(string token);
}