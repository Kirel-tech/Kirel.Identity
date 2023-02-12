using AutoMapper;
using Kirel.Identity.Client.Blazor.Pages.DTOs;
using Kirel.Identity.DTOs;

namespace Example.Blazor.Jwt.Mappings;

public class UniversalClaimMappings : Profile
{
    public UniversalClaimMappings()
    {
        CreateMap<UniversalClaimDto, KirelClaimDto>().ReverseMap();
        CreateMap<UniversalClaimDto, KirelClaimUpdateDto>().ReverseMap();
        CreateMap<UniversalClaimDto, KirelClaimCreateDto>().ReverseMap();
        CreateMap<KirelClaimDto, KirelClaimDto>().ReverseMap();
        CreateMap<KirelClaimDto, KirelClaimUpdateDto>().ReverseMap();
        CreateMap<KirelClaimDto, KirelClaimCreateDto>().ReverseMap();
        CreateMap<KirelClaimUpdateDto, KirelClaimCreateDto>().ReverseMap();
    }
}