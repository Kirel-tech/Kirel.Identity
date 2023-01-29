using AutoMapper;
using Example.API.Models;
using Example.DTOs;
using Kirel.Identity.Core.Services;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Example.API.Services;

/// <inheritdoc />
public class ExRoleService : KirelRoleService<Guid, ExRole, ExRoleDto, ExRoleCreateDto,ExRoleUpdateDto, KirelClaimDto,
    KirelClaimCreateDto, KirelClaimUpdateDto>
{
    /// <inheritdoc />
    public ExRoleService(RoleManager<ExRole> roleManager, IMapper mapper) : base(roleManager, mapper)
    {
    }
}