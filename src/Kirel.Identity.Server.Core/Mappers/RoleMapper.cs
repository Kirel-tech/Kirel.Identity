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
        CreateMap<Role, RoleDto>();
        CreateMap<RoleCreateDto, Role>();
        CreateMap<RoleUpdateDto, Role>();
    }
}