using Kirel.Identity.Core.Validators;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Kirel.Identity.Server.Core.Validators;

/// <inheritdoc />
public class UserCreateDtoValidator : KirelUserCreateDtoValidator<Guid, User, Role, UserRole, UserClaim, RoleClaim, UserCreateDto, ClaimCreateDto>
{
    /// <inheritdoc />
    public UserCreateDtoValidator(IOptions<IdentityOptions> identityOptions, UserManager<User> userManager,
        RoleManager<Role> roleManager) : base(identityOptions, userManager, roleManager)
    {
    }
}