using Kirel.Identity.Core.Validators;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Server.Core.Validators;

/// <inheritdoc />
public class RoleUpdateDtoValidator : KirelRoleUpdateDtoValidator<Guid, Role, RoleUpdateDto, ClaimUpdateDto>
{
    /// <inheritdoc />
    public RoleUpdateDtoValidator(RoleManager<Role> roleManager, IHttpContextAccessor httpContextAccessor) : base(
        roleManager, httpContextAccessor)
    {
    }
}