namespace Kirel.Identity.Client.Interfaces;

/// <summary>
/// Describes the authentication service methods for the Blazor client
/// </summary>
public interface IClientAuthenticationService
{
    /// <summary>
    /// Start user session by login with a password.
    /// </summary>
    /// <param name="type"> Authentication type </param>
    /// <param name="login"> User login </param>
    /// <param name="password"> User password </param>
    /// <returns> Awaitable task </returns>
    Task LoginByPasswordAsync(string type, string login, string password);

    /// <summary>
    /// Gets session expiration time.
    /// </summary>
    /// <returns> Task with DateTimeOffset </returns>
    Task<DateTimeOffset> GetSessionExpirationTimeAsync();

    /// <summary>
    /// Get session expiration time
    /// </summary>
    /// <returns> </returns>
    Task<bool> SessionIsActiveAsync();

    /// <summary>
    /// Extends the duration of an authentication session
    /// </summary>
    /// <returns> Session extension task </returns>
    Task ExtendSessionAsync();

    /// <summary>
    /// Closes the authentication session
    /// </summary>
    /// <returns> Authentication session closing task </returns>
    Task Logout();
}