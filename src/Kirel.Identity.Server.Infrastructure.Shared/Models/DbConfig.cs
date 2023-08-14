namespace Kirel.Identity.Server.Infrastructure.Shared.Models;

/// <summary>
/// Class that represents data base configuration
/// </summary>
public class DbConfig
{
    /// <summary>
    /// Database driver
    /// </summary>
    public string Driver { get; set; } = "";

    /// <summary>
    /// Database connection string parameters
    /// </summary>
    public DbParams Params { get; set; } = new();
}

/// <summary>
/// Database connection string parameters
/// </summary>
public class DbParams
{
    /// <summary>
    /// Database address
    /// </summary>
    public string Address { get; set; } = "";

    /// <summary>
    /// Database ethernet port
    /// </summary>
    public string Port { get; set; } = "";

    /// <summary>
    /// Name of the database
    /// </summary>
    public string DatabaseName { get; set; } = "";

    /// <summary>
    /// Database user login
    /// </summary>
    public string User { get; set; } = "";

    /// <summary>
    /// Database user password
    /// </summary>
    public string Password { get; set; } = "";
}