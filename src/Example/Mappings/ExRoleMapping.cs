using AutoMapper;
using Example.DTOs;
using Example.Models;

namespace Example.Mappings;

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