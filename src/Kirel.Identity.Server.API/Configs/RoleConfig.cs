namespace Kirel.Identity.Server.API.Configs;

/// <summary>
/// Configuration class for Role creation
/// </summary>
public class RoleConfig
{
    /// <summary>
    /// Role name
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Claims list of the role
    /// </summary>
    public List<ClaimConfig> Claims { get; set; } = new();
}