namespace Kirel.Identity.Server.Infrastructure.Models;

/// <summary>
/// Configuration for disposable codes
/// </summary>
public class DisposableCodesConfig
{
    /// <summary>
    /// Disposable codes expiration time
    /// </summary>
    public int ExpirationTime { get; set; } = 5;
}