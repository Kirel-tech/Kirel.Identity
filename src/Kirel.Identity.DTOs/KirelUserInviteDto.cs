namespace Kirel.Identity.DTOs;

/// <summary>
/// Dto for invite user
/// </summary>
public class KirelUserInviteDto
{
    /// <summary>
    /// Invited user email
    /// </summary>
    public string? Email { get; set; } = "";
    /// <summary>
    /// Invited user phone number
    /// </summary>
    public string? PhoneNumber { get; set; } = "";
}