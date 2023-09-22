using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Models;
using Kirel.Identity.Core.Services;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Controllers;

/// <summary>
/// Controller for user invitations
/// </summary>
/// <typeparam name="TUserInviteService"> The user invite service type</typeparam>
/// <typeparam name="TKey"> The user key type </typeparam>
/// <typeparam name="TUser"> The user type </typeparam>
/// <typeparam name="TRole"> The role entity type </typeparam>
/// <typeparam name="TUserRole"> The user role entity type </typeparam>
/// <typeparam name="TUserClaim"> User claim type. </typeparam>
/// <typeparam name="TRoleClaim"> Role claim type. </typeparam>
/// <typeparam name="TUserInviteDto"> The user invite dto type</typeparam>
/// <typeparam name="TInvitedUserDto"> The invited user dto type</typeparam>
[Authorize(AuthenticationSchemes = "Bearer")]
public class
    KirelInvitesController<TUserInviteService, TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim, TUserInviteDto, TInvitedUserDto> : Controller
    where TUserInviteService : KirelUserInviteService<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim, TUserInviteDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
    where TRoleClaim : KirelIdentityRoleClaim<TKey>
    where TUserClaim : KirelIdentityUserClaim<TKey>
    where TUserInviteDto: KirelUserInviteDto
    where TInvitedUserDto : KirelInvitedUserDto<TKey>
{
    /// <summary>
    /// Service for user invites
    /// </summary>
    protected readonly TUserInviteService UserInviteService;

    /// <summary>
    /// Constructor for KirelInvitesController
    /// </summary>
    /// <param name="userInviteService">User invite service</param>
    public KirelInvitesController(TUserInviteService userInviteService)
    {
        UserInviteService = userInviteService;
    }
    
    /// <summary>
    /// Invites user by email or sms
    /// </summary>
    /// <param name="inviteDto">User invite dto</param>
    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer, APIKey")]
    public virtual async Task<ActionResult<TInvitedUserDto>> InviteUser(TUserInviteDto inviteDto)
    {
        var result = await UserInviteService.InviteUser(inviteDto);
        return Ok(result);
    }
}