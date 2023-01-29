using Example.API.Models;
using Example.DTOs;
using Kirel.Identity.Core.Validators;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Example.API.Validators;

/// <inheritdoc />
public class ExRoleUpdateDtoValidator : KirelRoleUpdateDtoValidator<Guid, ExRole, ExRoleUpdateDto, KirelClaimUpdateDto>
{
    /// <inheritdoc />
    public ExRoleUpdateDtoValidator(RoleManager<ExRole> roleManager) : base(roleManager)
    {
    }
}