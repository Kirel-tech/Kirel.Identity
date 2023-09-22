using Kirel.Identity.Controllers;
using Kirel.Identity.Server.Core.Services;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Server.API.Controllers;

/// <inheritdoc />
[ApiController]
[Route("invited/registration")]
public class RegisterInvitedUserController : KirelRegisterInvitedUserController<RegistrationService, RegisterInvitedUserDto, UserRegistrationDto, Guid, User, Role, UserRole, UserClaim, RoleClaim>
{
    /// <inheritdoc />
    public RegisterInvitedUserController(RegistrationService service) : base(service)
    {
    }
}