namespace Kirel.Identity.Server.API.Configs;

/// <summary>
/// Configuration for create claim
/// </summary>
public class ClaimConfig
{
    /// <summary>
    /// Claim type
    /// </summary>
    public string Type { get; set; } = "";
    /// <summary>
    /// Claim value
    /// </summary>
    public string Value { get; set; } = "";
}