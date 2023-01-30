using Example.API.Models;
using Example.DTOs;
using Kirel.Identity.Core.Validators;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Example.API.Validators;

/// <inheritdoc />
public class ExUserUpdateDtoValidator : KirelUserUpdateDtoValidator<Guid, ExUser, ExRole, ExUserUpdateDto,
    KirelClaimUpdateDto>
{
    /// <inheritdoc />
    public ExUserUpdateDtoValidator(IOptions<IdentityOptions> identityOptions, UserManager<ExUser> userManager, RoleManager<ExRole> roleManager, IHttpContextAccessor httpContextAccessor) 
        : base(identityOptions, userManager, roleManager, httpContextAccessor)
    {
    }
}