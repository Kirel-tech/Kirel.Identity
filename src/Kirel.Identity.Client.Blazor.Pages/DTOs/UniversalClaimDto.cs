namespace Kirel.Identity.Client.Blazor.Pages.DTOs;

/// <summary>
/// Universal data transfer object for claim
/// </summary>
public class UniversalClaimDto
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