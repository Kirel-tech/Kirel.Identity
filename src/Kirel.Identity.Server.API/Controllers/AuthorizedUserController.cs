using AutoMapper;
using Kirel.Identity.Controllers;
using Kirel.Identity.Server.Core.Services;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Server.API.Controllers;

/// <inheritdoc />
[ApiController]
[Authorize]
[Route("authorized/user")]
public class AuthorizedUserController : KirelAuthorizedUserController<AuthorizedUserService, Guid, User, Role, UserRole,
    AuthorizedUserDto, AuthorizedUserUpdateDto>
{
    /// <inheritdoc />
    public AuthorizedUserController(AuthorizedUserService service, IMapper mapper) : base(service, mapper)
    {
    }
}