namespace Kirel.Identity.Server.Domain;

/// <summary>
/// Configuration class for DataSeed settings
/// </summary>
public class DataSeedConfig
{
    /// <summary>
    /// Gets or sets the list of role configurations.
    /// </summary>
    public List<RoleConfig> Roles { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of user configurations.
    /// </summary>
    public List<UserConfig> Users { get; set; } = new();
}