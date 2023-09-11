using AutoMapper;
using Kirel.Identity.Core.Services;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Server.Core.Services;

/// <inheritdoc />
public class UserService : KirelUserService<Guid, User, Role, UserRole, UserClaim, RoleClaim, UserDto, UserCreateDto, UserUpdateDto, ClaimDto,
    ClaimCreateDto, ClaimUpdateDto>
{
    /// <inheritdoc />
    public UserService(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper) : base(userManager, mapper)
    {
    }
}