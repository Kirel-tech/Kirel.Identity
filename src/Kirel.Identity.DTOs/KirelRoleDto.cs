namespace Kirel.Identity.DTOs;

/// <summary>
/// Data transfer object for role receive
/// </summary>
public class KirelRoleDto<TKey, TClaimDto> where TClaimDto : KirelClaimDto
{
    /// <summary>
    /// Role Id
    /// </summary>
    public TKey Id { get; set; } = default!;

    /// <summary>
    /// Role name
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// List of user claims
    /// </summary>
    public List<TClaimDto> Claims { get; set; } = new();
}