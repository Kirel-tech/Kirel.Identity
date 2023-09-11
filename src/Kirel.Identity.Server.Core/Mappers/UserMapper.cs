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
        CreateMap<User, UserDto>()
            .ForMember(d => d.Roles, 
            opt => opt.MapFrom(s => s.UserRoles.Select(ur => ur.RoleId)));
        CreateMap<User, AuthorizedUserDto>();
        CreateMap<UserCreateDto, User>().ForMember(d => d.UserRoles, opt => opt.MapFrom(
            s => s.Roles.Select(r => new UserRole() {RoleId = r})
        ));
        CreateMap<UserUpdateDto, User>().ForMember(d => d.UserRoles, 
        opt => opt.MapFrom(source => source.Roles.Select(id => new UserRole(){RoleId = id}).ToList())
        );
        CreateMap<UserRegistrationDto, User>();
        CreateMap<AuthorizedUserUpdateDto, User>();
    }
}