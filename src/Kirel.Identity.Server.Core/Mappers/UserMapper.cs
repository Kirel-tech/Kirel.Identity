using AutoMapper;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;

namespace Kirel.Identity.Server.Core.Mappers;

/// <summary>
/// Mapping profile for user entity
/// </summary>
public class UserMapper : Profile
{
    /// <summary>
    /// UserMapper constructor
    /// </summary>
    public UserMapper()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserCreateDto>().ReverseMap();
        CreateMap<User, UserUpdateDto>().ReverseMap();
        CreateMap<User, UserRegistrationDto>().ReverseMap();
        CreateMap<User, AuthorizedUserDto>().ReverseMap();
        CreateMap<User, AuthorizedUserUpdateDto>().ReverseMap();
    }
}