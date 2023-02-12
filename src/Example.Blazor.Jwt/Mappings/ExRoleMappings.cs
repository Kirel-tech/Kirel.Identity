using AutoMapper;
using Example.DTOs;

namespace Example.Blazor.Jwt.Mappings;

public class ExRoleMappings : Profile
{
    public ExRoleMappings()
    {
        CreateMap<ExRoleDto, ExRoleDto>();
        CreateMap<ExRoleDto, ExRoleUpdateDto>();
    }
}