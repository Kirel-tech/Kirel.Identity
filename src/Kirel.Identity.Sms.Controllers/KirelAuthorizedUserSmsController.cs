using System.ComponentModel.DataAnnotations;
using Kirel.Identity.Core.Models;
using Kirel.Identity.Core.Services;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Sms.Controllers;

/// <summary>
/// Controller for authorized user phone confirmation
/// </summary>
/// <typeparam name="TAuthorizedUserService">Authorized user service type</typeparam>
/// <typeparam name="TSmsConfirmationService">Sms confirmation service type</typeparam>
/// <typeparam name="TKey"> The user key type </typeparam>
/// <typeparam name="TUser"> The user type </typeparam>
/// <typeparam name="TRole"> The role entity type </typeparam>
/// <typeparam name="TUserRole"> The user role entity type </typeparam>
/// <typeparam name="TAuthorizedUserDto">Authorized user dto type</typeparam>
/// <typeparam name="TAuthorizedUserUpdateDto">Authorized user update dto type</typeparam>
public class KirelAuthorizedUserSmsController<TAuthorizedUserService, TSmsConfirmationService, TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim,
    TAuthorizedUserDto, TAuthorizedUserUpdateDto> : Controller
    where TAuthorizedUserService : KirelAuthorizedUserService<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim, TAuthorizedUserDto, TAuthorizedUserUpdateDto>
    where TSmsConfirmationService : KirelSmsConfirmationService<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
    where TAuthorizedUserDto : KirelAuthorizedUserDto
    where TAuthorizedUserUpdateDto : KirelAuthorizedUserUpdateDto
    where TRoleClaim : KirelIdentityRoleClaim<TKey>
    where TUserClaim : KirelIdentityUserClaim<TKey>

{
    /// <summary>
    /// Authorized user service. Must be a descendant of the KirelAuthorizedUserService class
    /// </summary>
    protected readonly TAuthorizedUserService AuthorizedUserService;
    
    /// <summary>
    /// Authorized user service. Must be a descendant of the KirelAuthorizedUserService class
    /// </summary>
    protected readonly TSmsConfirmationService SmsConfirmationService;

    /// <summary>
    /// KirelAuthorizedUserSmsController constructor
    /// </summary>
    /// <param name="authorizedService"> Service for authorized user account management </param>
    /// <param name="confirmationService"> Service for phone number confirmation </param>
    public KirelAuthorizedUserSmsController(TAuthorizedUserService authorizedService, TSmsConfirmationService confirmationService)
    {
        AuthorizedUserService = authorizedService;
        SmsConfirmationService = confirmationService;
    }

    /// <summary>
    /// Send confirmation code to the authorized user phone number
    /// </summary>
    [HttpPost("phone/confirm")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public virtual async Task<ActionResult> SendConfirmationToken()
    {
        var user = await AuthorizedUserService.Get();
        await SmsConfirmationService.SendConfirmationSms(user);
        return NoContent();
    }
    
    /// <summary>
    /// Confirm the phone number for authorized user by code
    /// </summary>
    /// <param name="code"></param>
    [HttpPut("phone/confirm")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public virtual async Task<ActionResult> ConfirmPhoneNumber([FromQuery] string code)
    {
        var user = await AuthorizedUserService.Get();
        await SmsConfirmationService.ConfirmPhoneNumber(user, code);
        return NoContent();
    }
}