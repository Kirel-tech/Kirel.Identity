using Kirel.Identity.Controllers;
using Kirel.Identity.Server.Core.Services;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;
using Kirel.Identity.Server.Infrastructure.Managers;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Server.API.Controllers;

/// <inheritdoc />
[Route("invites")]
[ApiController]
public class UserInviteController : KirelInvitesController<UserInviteService,
    Guid, User, Role, UserRole, UserClaim, RoleClaim, UserInviteDto, InvitedUserDto>
{
    /// <inheritdoc />
    public UserInviteController(UserInviteService userInviteService) : base(userInviteService)
    {
    }
}