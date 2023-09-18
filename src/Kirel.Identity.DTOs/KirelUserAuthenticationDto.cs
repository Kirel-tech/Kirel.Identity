namespace Kirel.Identity.DTOs;

/// <summary>
/// Dto for user authentication
/// </summary>
public class KirelUserAuthenticationDto
{
    /// <summary>
    /// Login type can be one of the following: 'Username', 'Phone' or 'Email'.
    /// </summary>
    public string Type { get; set; } = "";
    /// <summary>
    /// User login can be one of the following: Username, Phone number or Email.
    /// For authentication by number or email those must be confirmed
    /// </summary>
    public string Login { get; set; } = "";
    /// <summary>
    /// User password or token from phone/email based on authentication type
    /// </summary>
    public string Password { get; set; } = "";
}