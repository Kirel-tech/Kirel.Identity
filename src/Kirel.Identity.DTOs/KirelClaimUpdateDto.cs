namespace Kirel.Identity.DTOs;

/// <summary>
/// Data transfer object for claim update
/// </summary>
public class KirelClaimUpdateDto
{
    /// <summary>
    /// Claim type
    /// </summary>
    public string Type { get; set; } = "";

    /// <summary>
    /// Claim value
    /// </summary>
    public string Value { get; set; } = "";
}