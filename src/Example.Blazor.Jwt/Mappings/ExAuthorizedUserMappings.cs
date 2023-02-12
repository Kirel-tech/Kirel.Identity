using AutoMapper;
using Example.DTOs;

namespace Example.Blazor.Jwt.Mappings;

public class ExAuthorizedUserMappings : Profile
{
    public ExAuthorizedUserMappings()
    {
        CreateMap<ExAuthorizedUserDto, ExAuthorizedUserUpdateDto>().ReverseMap();
    }
}