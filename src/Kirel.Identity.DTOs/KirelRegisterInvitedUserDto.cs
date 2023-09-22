namespace Kirel.Identity.DTOs;

/// <summary>
/// Dto for registration invited user
/// </summary>
public class KirelRegisterInvitedUserDto
{
    /// <summary>
    /// First name of the user
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Last name of the user
    /// </summary>
    public string LastName { get; set; } = "";

    /*/// <summary>
    /// User email
    /// </summary>
    public string? Email { get; set; } = "";

    /// <summary>
    /// User phone number
    /// </summary>
    public string? PhoneNumber { get; set; } = "";*/

    /// <summary>
    /// User password
    /// </summary>
    public string Password { get; set; } = "";
}