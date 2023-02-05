namespace Kirel.Identity.DTOs;

/// <summary>
/// Data transfer object for creating a new role
/// </summary>
public class KirelRoleCreateDto<TClaimCreateDto> where TClaimCreateDto : KirelClaimCreateDto
{
    /// <summary>
    /// Role name
    /// </summary>
    public string Name { get; set; } = "";
    /// <summary>
    /// List of user claims
    /// </summary>
    public List<TClaimCreateDto> Claims { get; set; }  = new ();
}