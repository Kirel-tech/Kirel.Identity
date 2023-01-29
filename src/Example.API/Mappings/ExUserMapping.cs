using AutoMapper;
using Example.API.Models;
using Example.DTOs;

namespace Example.API.Mappings;

/// <summary>
/// Mapping profile for user
/// </summary>
public class ExUserMapping : Profile
{
    /// <summary>
    /// UserMapping constructor
    /// </summary>
    public ExUserMapping()
    {
        CreateMap<ExUser, ExUserDto>().ReverseMap();
        CreateMap<ExUser, ExUserCreateDto>().ReverseMap();
        CreateMap<ExUser, ExUserUpdateDto>().ReverseMap();
        CreateMap<ExUser, ExUserRegistrationDto>().ReverseMap();
        CreateMap<ExUser, ExAuthorizedUserDto>().ReverseMap();
        CreateMap<ExUser, ExAuthorizedUserUpdateDto>().ReverseMap();
    }
}