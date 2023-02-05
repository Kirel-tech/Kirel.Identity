namespace Kirel.Identity.DTOs;

/// <summary>
/// DTO for return existing user
/// </summary>
public class KirelUserDto<TKey, TRoleKey, TClaimDto> where TClaimDto : KirelClaimDto
{
    /// <summary>
    /// User id
    /// </summary>
    public TKey Id { get; set; }
    /// <summary>
    /// User create date and time
    /// </summary>
    public DateTime Created { get; set; }
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

    /// <summary>
    /// Gets or sets a flag indicating if the user could be locked out.
    /// </summary>
    /// <value>True if the user could be locked out, otherwise false.</value>
    public bool LockoutEnabled { get; set; } = false;

    /// <summary>
    /// List of user roles
    /// </summary>
    public List<TRoleKey> Roles { get; set; } = new ();
    /// <summary>
    /// List of user claims
    /// </summary>
    public List<TClaimDto> Claims { get; set; } = new ();
}