namespace Kirel.Identity.DTOs;

/// <summary>
/// Data transfer object for update existing user
/// </summary>
public class KirelUserUpdateDto<TRoleKey, TClaimUpdateDto> where TClaimUpdateDto : KirelClaimUpdateDto
{
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

    /// <summary>
    /// User password
    /// </summary>
    public string Password { get; set; } = "";

    /// <summary>
    /// Gets or sets the date and time, in UTC, when any user lockout ends.
    /// </summary>
    /// <remarks>
    /// A value in the past means the user is not locked out.
    /// </remarks>
    public DateTimeOffset? LockoutEnd { get; set; } = DateTimeOffset.MinValue;

    /// <summary>
    /// Gets or sets a flag indicating if the user could be locked out.
    /// </summary>
    /// <value> True if the user could be locked out, otherwise false. </value>
    public bool LockoutEnabled { get; set; } = false;

    /// <summary>
    /// List of user roles
    /// </summary>
    public List<TRoleKey> Roles { get; set; } = new();

    /// <summary>
    /// List of user claims
    /// </summary>
    public List<TClaimUpdateDto> Claims { get; set; } = new();
    /// <summary>
    /// Is user finished his registration
    /// </summary>
    public bool IsRegistrationFinished { get; set; } = false;
}