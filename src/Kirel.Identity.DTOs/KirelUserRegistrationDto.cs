namespace Kirel.Identity.DTOs;

/// <summary>
/// Data transfer object for user registration
/// </summary>
public class KirelUserRegistrationDto
{
    /// <summary>
    /// Username
    /// </summary>
    public string UserName { get; set; } = "";
    /// <summary>
    /// First name of the user
    /// </summary>
    public string Name { get; set; } = "";
    /// <summary>
    /// Last name of the user
    /// </summary>
    public string LastName { get; set; } = "";
    /// <summary>
    /// User email
    /// </summary>
    public string Email { get; set; } = "";
    /// <summary>
    /// User phone number
    /// </summary>
    public string PhoneNumber { get; set; } = "";
    /// <summary>
    /// User password
    /// </summary>
    public string Password { get; set; } = "";
}