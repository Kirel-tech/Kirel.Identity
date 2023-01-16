using Example.DTOs;
using Example.Models;
using Kirel.Identity.Core.Validators;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Example.Validators;

/// <inheritdoc />
public class ExRoleUpdateDtoValidator : KirelRoleUpdateDtoValidator<Guid, ExRole,ExRoleUpdateDto,KirelClaimUpdateDto>
{
    /// <inheritdoc />
    public ExRoleUpdateDtoValidator(RoleManager<ExRole> roleManager) : base(roleManager)
    {
    }
}