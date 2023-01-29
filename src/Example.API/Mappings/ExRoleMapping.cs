using AutoMapper;
using Example.API.Models;
using Example.DTOs;

namespace Example.API.Mappings;

/// <summary>
/// Mapping profile for role
/// </summary>
public class ExRoleMapping : Profile
{
    /// <summary>
    /// Role mapping constructor
    /// </summary>
    public ExRoleMapping()
    {
        CreateMap<ExRole, ExRoleDto>().ReverseMap();
        CreateMap<ExRole, ExRoleCreateDto>().ReverseMap();
        CreateMap<ExRole, ExRoleUpdateDto>().ReverseMap();
    }
}