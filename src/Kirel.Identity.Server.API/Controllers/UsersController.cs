using Kirel.Identity.Controllers;
using Kirel.Identity.Server.Core.Services;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Server.API.Controllers;

/// <inheritdoc />
[Route("users")]
[ApiController]
public class UsersController : KirelUsersController<UserService, Guid, User, Role, UserRole, UserDto, UserCreateDto, UserUpdateDto
    , ClaimDto, ClaimCreateDto, ClaimUpdateDto>
{
    /// <inheritdoc />
    public UsersController(UserService service) : base(service)
    {
    }
}