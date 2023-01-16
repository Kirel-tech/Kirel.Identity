using Example.DTOs;
using Example.Models;
using Kirel.Identity.Core.Validators;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Example.Validators;

/// <inheritdoc />
public class ExRoleCreateDtoValidator : KirelRoleCreateDtoValidator<Guid, ExRole, ExRoleCreateDto, KirelClaimCreateDto>
{
    /// <inheritdoc />
    public ExRoleCreateDtoValidator(RoleManager<ExRole> roleManager) : base(roleManager)
    {
    }
}