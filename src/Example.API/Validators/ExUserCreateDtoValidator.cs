using Example.API.Models;
using Example.DTOs;
using Kirel.Identity.Core.Validators;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Example.API.Validators;

/// <inheritdoc />
public class ExUserCreateDtoValidator : KirelUserCreateDtoValidator<Guid, ExUser, ExRole, ExUserCreateDto,
    KirelClaimCreateDto>
{
    /// <inheritdoc />
    public ExUserCreateDtoValidator(IOptions<IdentityOptions> identityOptions, UserManager<ExUser> userManager, RoleManager<ExRole> roleManager) : base(identityOptions, userManager, roleManager)
    {
    }
}