namespace Kirel.Identity.DTOs;

/// <summary>
/// Data transfer object for role update
/// </summary>
public class KirelRoleUpdateDto<TClaimUpdateDto> where TClaimUpdateDto : KirelClaimUpdateDto
{
    /// <summary>
    /// Role name
    /// </summary>
    public string Name { get; set; }  = "";
    /// <summary>
    /// List of user claims
    /// </summary>
    public List<TClaimUpdateDto> Claims { get; set; }  = new ();
}