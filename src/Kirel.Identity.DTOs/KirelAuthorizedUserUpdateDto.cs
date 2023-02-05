namespace Kirel.Identity.DTOs;

/// <summary>
/// Data transfer object for authorized user update
/// </summary>
public class KirelAuthorizedUserUpdateDto
{
    /// <summary>
    /// First name of the user
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Last name of the user
    /// </summary>
    public string LastName { get; set; } = "";
}