using Example.API.DTOs;
using Example.API.Models;
using Example.API.Services;
using Kirel.Identity.Controllers;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Example.API.Controllers;

/// <inheritdoc />
[ApiController]
[Authorize]
[Route("users")]
public class ExUsersController : KirelUsersController<ExUserService, Guid, ExUser, ExRole, ExUserDto, ExUserCreateDto,
    ExUserUpdateDto, KirelClaimDto, KirelClaimCreateDto, KirelClaimUpdateDto>
{
    /// <inheritdoc />
    public ExUsersController(ExUserService service) : base(service)
    {
    }
}