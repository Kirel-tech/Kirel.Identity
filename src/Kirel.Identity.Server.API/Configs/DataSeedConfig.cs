using Kirel.Identity.Server.Domain;

namespace Kirel.Identity.Server.API.Configs;

/// <summary>
/// Configuration class for identity data seed settings
/// </summary>
public class IdentityDataSeedConfig
{
    /// <summary>
    /// Gets or sets the list of role configurations.
    /// </summary>
    public List<string> Roles { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of user configurations.
    /// </summary>
    public List<UserConfig> Users { get; set; } = new();
}