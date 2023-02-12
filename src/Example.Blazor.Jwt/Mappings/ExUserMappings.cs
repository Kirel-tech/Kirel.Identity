using AutoMapper;
using Example.DTOs;

namespace Example.Blazor.Jwt.Mappings;

public class ExUserMappings : Profile
{
    public ExUserMappings()
    {
        CreateMap<ExUserDto, ExUserDto>();
        CreateMap<ExUserDto, ExUserUpdateDto>();
    }
}