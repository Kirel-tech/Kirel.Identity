using Kirel.Identity.Core.Validators;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Kirel.Identity.Server.Core.Validators;

/// <inheritdoc />
public class UserUpdateDtoValidator : KirelUserUpdateDtoValidator<Guid, User, Role, UserUpdateDto, ClaimUpdateDto>
{
    /// <inheritdoc />
    public UserUpdateDtoValidator(IOptions<IdentityOptions> identityOptions, UserManager<User> userManager,
        RoleManager<Role> roleManager, IHttpContextAccessor httpContextAccessor) : base(identityOptions, userManager,
        roleManager, httpContextAccessor)
    {
    }
}