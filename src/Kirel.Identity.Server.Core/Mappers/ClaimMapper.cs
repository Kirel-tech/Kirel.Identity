using System.Security.Claims;
using AutoMapper;
using Kirel.Identity.Server.DTOs;

namespace Kirel.Identity.Server.Core.Mappers;

/// <summary>
/// Mapping profile for claim
/// </summary>
public class ClaimMapper : Profile
{
    /// <summary>
    /// ClaimMapper constructor
    /// </summary>
    public ClaimMapper()
    {
        CreateMap<Claim, ClaimDto>().ReverseMap();
        CreateMap<Claim, ClaimCreateDto>().ReverseMap();
        CreateMap<Claim, ClaimUpdateDto>().ReverseMap();
    }
}