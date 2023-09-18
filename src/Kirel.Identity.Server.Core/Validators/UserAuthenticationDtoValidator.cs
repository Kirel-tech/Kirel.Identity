using Kirel.Identity.Core.Validators;
using Kirel.Identity.DTOs;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Server.Core.Validators;

/// <inheritdoc />
public class UserAuthenticationDtoValidator : KirelUserAuthenticationDtoValidator<Guid, User, Role, UserRole, UserAuthenticationDto, UserClaim, RoleClaim>
{
    /// <inheritdoc />
    public UserAuthenticationDtoValidator(UserManager<User> userManager) : base(userManager)
    {
    }
}