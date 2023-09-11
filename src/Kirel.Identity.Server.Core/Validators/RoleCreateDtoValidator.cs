using Kirel.Identity.Core.Validators;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Server.Core.Validators;

/// <inheritdoc />
public class RoleCreateDtoValidator : KirelRoleCreateDtoValidator<Guid, Role, User, UserRole, RoleClaim, UserClaim, RoleCreateDto, ClaimCreateDto>
{
    /// <inheritdoc />
    public RoleCreateDtoValidator(RoleManager<Role> roleManager) : base(roleManager)
    {
    }
}