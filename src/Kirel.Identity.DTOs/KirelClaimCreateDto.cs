namespace Kirel.Identity.DTOs;

/// <summary>
/// Data transfer object for create new claim
/// </summary>
public class KirelClaimCreateDto
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