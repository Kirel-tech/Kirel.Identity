using Example.API.Models;
using Example.DTOs;
using Kirel.Identity.Core.Validators;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Example.API.Validators;

/// <inheritdoc />
public class ExRoleCreateDtoValidator : KirelRoleCreateDtoValidator<Guid, ExRole, ExRoleCreateDto, KirelClaimCreateDto>
{
    /// <inheritdoc />
    public ExRoleCreateDtoValidator(RoleManager<ExRole> roleManager) : base(roleManager)
    {
    }
}