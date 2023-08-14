namespace Kirel.Identity.DTOs;

/// <summary>
/// Data transfer object for authorized user receive
/// </summary>
public class KirelAuthorizedUserDto
{
    /// <summary>
    /// User creation date and time
    /// </summary>
    public DateTime Created { get; set; } = DateTime.MinValue;

    /// <summary>
    /// User last update date and time
    /// </summary>
    public DateTime? Updated { get; set; }

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
    /// Flag whether the email was verified
    /// </summary>
    public bool EmailConfirmed { get; set; } = false;

    /// <summary>
    /// User phone number
    /// </summary>
    public string PhoneNumber { get; set; } = "";

    /// <summary>
    /// Flag whether the phone number was verified
    /// </summary>
    public bool PhoneNumberConfirmed { get; set; } = false;
}