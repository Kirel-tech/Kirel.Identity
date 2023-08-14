namespace Kirel.Identity.Server.Domain;

/// <summary>
/// Maintenance config parameters
/// </summary>
public class MaintenanceConfig
{
    /// <summary>
    /// Root admin parameters
    /// </summary>
    public AdminConfig Admin { get; set; } = new();
}

/// <summary>
/// Root admin parameters
/// </summary>
public class AdminConfig
{
    /// <summary>
    /// If true, admin password will be reset
    /// </summary>
    public bool Reset { get; set; }

    /// <summary>
    /// Password which will be set after reset
    /// </summary>
    public string Password { get; set; } = "";
}