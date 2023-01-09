namespace Kirel.Identity.DTOs;

/// <summary>
/// Data transfer object for update existing user
/// </summary>
public class KirelUserUpdateDto<TRoleKey, TClaimUpdateDto> where TClaimUpdateDto : KirelClaimUpdateDto
{
    /// <summary>
    /// First name of the user
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Last name of the user
    /// </summary>
    public string LastName { get; set; }
    /// <summary>
    /// User email
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// Flag whether the email was verified
    /// </summary>
    public bool EmailConfirmed { get; set; }
    /// <summary>
    /// User phone number
    /// </summary>
    public string PhoneNumber { get; set; }
    /// <summary>
    /// Flag whether the phone number was verified
    /// </summary>
    public bool PhoneNumberConfirmed { get; set; }
    /// <summary>
    /// User password
    /// </summary>
    public string Password { get; set; }
    /// <summary>
    /// Gets or sets a flag indicating if the user could be locked out.
    /// </summary>
    /// <value>True if the user could be locked out, otherwise false.</value>
    public bool LockoutEnabled { get; set; }
    /// <summary>
    /// List of user roles
    /// </summary>
    public List<TRoleKey> Roles { get; set; }
    /// <summary>
    /// List of user claims
    /// </summary>
    public List<TClaimUpdateDto> Claims { get; set; }
}