using Example.DTOs;
using Example.Models;
using Kirel.Identity.Core.Validators;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Example.Validators;

/// <inheritdoc />
public class ExUserCreateDtoValidator : KirelUserCreateDtoValidator<Guid, ExUser, ExRole, ExUserCreateDto,
    KirelClaimCreateDto>
{
    /// <inheritdoc />
    public ExUserCreateDtoValidator(UserManager<ExUser> userManager, RoleManager<ExRole> roleManager) : base(userManager, roleManager)
    {
    }
}