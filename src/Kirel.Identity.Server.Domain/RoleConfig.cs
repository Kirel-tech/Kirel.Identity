namespace Kirel.Identity.Server.Domain;

/// <summary>
/// Configuration class for roles
/// </summary>
public class RoleConfig
{
    /// <summary>
    /// Gets or sets the role name.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Gets or sets the normalized role name.
    /// </summary>
    public string NormalizedName { get; set; } = "";
}