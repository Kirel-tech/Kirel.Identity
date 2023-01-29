using Example.API.Models;
using Example.DTOs;
using Kirel.Identity.Core.Validators;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Example.API.Validators;

/// <inheritdoc />
public class ExUserUpdateDtoValidator : KirelUserUpdateDtoValidator<Guid, ExUser, ExRole, ExUserUpdateDto,
    KirelClaimUpdateDto>
{
    /// <inheritdoc />
    public ExUserUpdateDtoValidator(UserManager<ExUser> userManager, RoleManager<ExRole> roleManager, IHttpContextAccessor httpContextAccessor) : base(userManager, roleManager, httpContextAccessor)
    {
    }
}