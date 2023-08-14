using AutoMapper;
using Kirel.Identity.Server.DTOs;

namespace Kirel.Identity.Blazor.GUI.Mappings;

public class ExAuthorizedUserMappings : Profile
{
    public ExAuthorizedUserMappings()
    {
        CreateMap<AuthorizedUserDto, AuthorizedUserUpdateDto>().ReverseMap();
    }
}