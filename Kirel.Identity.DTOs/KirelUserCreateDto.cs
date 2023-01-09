
namespace Kirel.Identity.DTOs;

/// <summary>
/// Data transfer object for create new user
/// </summary>
public class KirelUserCreateDto<TRoleKey, TClaimCreateDto> where TClaimCreateDto : KirelClaimCreateDto
{
    /// <summary>
    /// Unique user name
    /// </summary>
    public string UserName { get; set; }
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
    /// List of user roles
    /// </summary>
    public List<TRoleKey> Roles { get; set; }
    /// <summary>
    /// List of user claims
    /// </summary>
    public List<TClaimCreateDto> Claims { get; set; }
}