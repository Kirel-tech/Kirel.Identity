using AutoMapper;
using Kirel.Identity.Server.DTOs;

namespace Kirel.Identity.Blazor.GUI.Mappings;

public class ExRoleMappings : Profile
{
    public ExRoleMappings()
    {
        CreateMap<RoleDto, RoleDto>();
        CreateMap<RoleDto, RoleUpdateDto>();
    }
}