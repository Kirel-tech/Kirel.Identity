namespace Kirel.Identity.Server.Domain;

/// <summary>
/// Configuration class for maintenance settings
/// </summary>
public class MaintenanceConfig
{
    /// <summary>
    /// Gets or sets the admin configuration.
    /// </summary>
    public AdminConfig Admin { get; set; } = new();
}

/// <summary>
/// Configuration class for admin settings
/// </summary>
public class AdminConfig
{
    /// <summary>
    /// Gets or sets a value indicating whether the admin password should be reset.
    /// </summary>
    public bool Reset { get; set; }

    /// <summary>
    /// Gets or sets the password which will be set after reset.
    /// </summary>
    public string Password { get; set; } = "";
}