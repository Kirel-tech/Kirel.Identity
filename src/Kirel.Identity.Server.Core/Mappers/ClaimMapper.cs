using System.Security.Claims;
using AutoMapper;
using Kirel.Identity.Server.Domain;
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
        CreateMap<Claim, ClaimDto>()
            .ForMember(s => s.Type, opt => opt.MapFrom(d => d.Type))
            .ForMember(s => s.Value, opt => opt.MapFrom(d => d.Value));
        CreateMap<UserClaim, ClaimDto>()
            .ForMember(s => s.Type, opt => opt.MapFrom(d => d.ClaimType))
            .ForMember(s => s.Value, opt => opt.MapFrom(d => d.ClaimValue));
        CreateMap<RoleClaim, ClaimDto>()
            .ForMember(s => s.Type, opt => opt.MapFrom(d => d.ClaimType))
            .ForMember(s => s.Value, opt => opt.MapFrom(d => d.ClaimValue));
        
        CreateMap<ClaimCreateDto, Claim>()
            .ForMember(s => s.Type, opt => opt.MapFrom(d => d.Type))
            .ForMember(s => s.Value, opt => opt.MapFrom(d => d.Value));
            
        CreateMap<ClaimCreateDto, UserClaim>()
            .ForMember(s => s.ClaimType, opt => opt.MapFrom(d => d.Type))
            .ForMember(s => s.ClaimValue, opt => opt.MapFrom(d => d.Value));
        CreateMap<ClaimCreateDto, RoleClaim>()
            .ForMember(s => s.ClaimType, opt => opt.MapFrom(d => d.Type))
            .ForMember(s => s.ClaimValue, opt => opt.MapFrom(d => d.Value));

        CreateMap<ClaimUpdateDto, Claim>()
            .ForMember(s => s.Type, opt => opt.MapFrom(d => d.Type))
            .ForMember(s => s.Value, opt => opt.MapFrom(d => d.Value));
        CreateMap<ClaimUpdateDto, UserClaim>()
            .ForMember(s => s.ClaimType, opt => opt.MapFrom(d => d.Type))
            .ForMember(s => s.ClaimValue, opt => opt.MapFrom(d => d.Value));
        CreateMap<ClaimUpdateDto, RoleClaim>()
            .ForMember(s => s.ClaimType, opt => opt.MapFrom(d => d.Type))
            .ForMember(s => s.ClaimValue, opt => opt.MapFrom(d => d.Value));
    }
}