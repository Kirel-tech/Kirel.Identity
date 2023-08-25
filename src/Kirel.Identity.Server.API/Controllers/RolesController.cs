using Kirel.Identity.Controllers;
using Kirel.Identity.Server.Core.Services;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Server.API.Controllers;

/// <inheritdoc />
[Route("roles")]
[ApiController]
public class RolesController : KirelRolesController<RoleService, Guid, Role, User, UserRole, RoleDto, RoleCreateDto, RoleUpdateDto,
    ClaimDto, ClaimCreateDto, ClaimUpdateDto>
{
    /// <inheritdoc />
    public RolesController(RoleService service) : base(service)
    {
    }
}