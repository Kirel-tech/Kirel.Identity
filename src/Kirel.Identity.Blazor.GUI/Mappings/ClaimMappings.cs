using AutoMapper;
using Kirel.Identity.Client.Blazor.Pages.DTOs;
using Kirel.Identity.Server.DTOs;

namespace Kirel.Identity.Blazor.GUI.Mappings;

public class UniversalClaimMappings : Profile
{
    public UniversalClaimMappings()
    {
        CreateMap<UniversalClaimDto, ClaimDto>().ReverseMap();
        CreateMap<UniversalClaimDto, ClaimUpdateDto>().ReverseMap();
        CreateMap<UniversalClaimDto, ClaimCreateDto>().ReverseMap();
        CreateMap<ClaimDto, ClaimDto>().ReverseMap();
        CreateMap<ClaimDto, ClaimUpdateDto>().ReverseMap();
        CreateMap<ClaimDto, ClaimCreateDto>().ReverseMap();
        CreateMap<ClaimUpdateDto, ClaimCreateDto>().ReverseMap();
    }
}