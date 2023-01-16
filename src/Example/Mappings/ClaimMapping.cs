using System.Security.Claims;
using AutoMapper;
using Kirel.Identity.DTOs;

namespace Example.Mappings;

/// <summary>
/// Mapping profile for claim
/// </summary>
public class ClaimMappings : Profile
{
    /// <summary>
    /// ClaimMappings constructor
    /// </summary>
    public ClaimMappings()
    {
        CreateMap<Claim, KirelClaimDto>().ReverseMap();
        CreateMap<Claim, KirelClaimCreateDto>().ReverseMap();
        CreateMap<Claim, KirelClaimUpdateDto>().ReverseMap();
    }
}