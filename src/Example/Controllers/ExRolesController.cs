using Example.DTOs;
using Example.Models;
using Example.Services;
using Kirel.Identity.Controllers;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers;

/// <inheritdoc />
[ApiController]
[Authorize]
[Route("roles")]
public class ExRolesController : KirelRolesController<ExRoleService, Guid, ExRole, ExRoleDto, ExRoleCreateDto,
    ExRoleUpdateDto,KirelClaimDto,KirelClaimCreateDto,KirelClaimUpdateDto>
{
    /// <inheritdoc />
    public ExRolesController(ExRoleService service) : base(service)
    {
    }
}