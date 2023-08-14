using AutoMapper;
using Kirel.Identity.Core.Services;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Server.Core.Services;

/// <inheritdoc />
public class RoleService : KirelRoleService<Guid, Role, RoleDto, RoleCreateDto, RoleUpdateDto, ClaimDto, ClaimCreateDto,
    ClaimUpdateDto>
{
    /// <inheritdoc />
    public RoleService(RoleManager<Role> roleManager, IMapper mapper) : base(roleManager, mapper)
    {
    }
}