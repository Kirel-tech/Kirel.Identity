using Example.DTOs;
using Example.Models;
using Kirel.Identity.Core.Validators;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Example.Validators;

/// <inheritdoc />
public class ExUserUpdateDtoValidator : KirelUserUpdateDtoValidator<Guid, ExUser, ExRole, ExUserUpdateDto,
    KirelClaimUpdateDto>
{
    /// <inheritdoc />
    public ExUserUpdateDtoValidator(UserManager<ExUser> userManager, RoleManager<ExRole> roleManager, IHttpContextAccessor httpContextAccessor) : base(userManager, roleManager, httpContextAccessor)
    {
    }
}