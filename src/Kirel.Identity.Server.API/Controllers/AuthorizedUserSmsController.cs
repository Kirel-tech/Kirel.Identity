using Kirel.Identity.Server.Core.Services;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;
using Kirel.Identity.Sms.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Server.API.Controllers;

/// <inheritdoc />
[Route("authorized/user")]
[ApiController]
public class UserPhoneConfirmationController : KirelUserPhoneConfirmationController<AuthorizedUserService, SmsConfirmationService, 
    Guid, User, Role, UserRole, UserClaim, RoleClaim, AuthorizedUserDto, AuthorizedUserUpdateDto>
    
{
    /// <inheritdoc />
    public UserPhoneConfirmationController(AuthorizedUserService authorizedService, SmsConfirmationService confirmationService) : base(authorizedService, confirmationService)
    {
    }
} 