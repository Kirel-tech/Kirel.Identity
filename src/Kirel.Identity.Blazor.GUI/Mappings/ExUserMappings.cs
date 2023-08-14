using AutoMapper;
using Kirel.Identity.Server.DTOs;

namespace Kirel.Identity.Blazor.GUI.Mappings;

public class ExUserMappings : Profile
{
    public ExUserMappings()
    {
        CreateMap<UserDto, UserDto>();
        CreateMap<UserDto, UserUpdateDto>();
    }
}