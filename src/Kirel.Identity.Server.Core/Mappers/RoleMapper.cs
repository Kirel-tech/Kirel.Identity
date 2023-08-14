using AutoMapper;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;

namespace Kirel.Identity.Server.Core.Mappers;

/// <summary>
/// Mapping profile for role entity
/// </summary>
public class RoleMapper : Profile
{
    /// <summary>
    /// RoleMapper constructor
    /// </summary>
    public RoleMapper()
    {
        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<Role, RoleCreateDto>().ReverseMap();
        CreateMap<Role, RoleUpdateDto>().ReverseMap();
    }
}