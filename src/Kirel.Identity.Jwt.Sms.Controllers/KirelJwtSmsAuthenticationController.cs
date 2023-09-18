using System.ComponentModel.DataAnnotations;
using Kirel.Identity.Core.Models;
using Kirel.Identity.Core.Services;
using Kirel.Identity.DTOs;
using Kirel.Identity.Jwt.Core.Services;
using Kirel.Identity.Jwt.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Jwt.Sms.Controllers;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TTokenService">Token service which is KirelJwtTokenService</typeparam>
/// <typeparam name="TAuthenticationService"></typeparam>
/// <typeparam name="TAuthorizedUserService"></typeparam>
/// <typeparam name="TKey"> The user key type </typeparam>
/// <typeparam name="TUser"> The user type </typeparam>
/// <typeparam name="TRole"> The role entity type </typeparam>
/// <typeparam name="TUserRole"> The user role entity type </typeparam>
/// <typeparam name="TAuthorizedUserDto"></typeparam>
/// <typeparam name="TAuthorizedUserUpdateDto"></typeparam>
public class KirelJwtSmsAuthenticationController<TTokenService, TAuthenticationService, TAuthorizedUserService,
    TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim, TAuthorizedUserDto, TAuthorizedUserUpdateDto> : Controller
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
    where TAuthorizedUserDto : KirelAuthorizedUserDto
    where TAuthorizedUserUpdateDto : KirelAuthorizedUserUpdateDto
    where TTokenService : KirelJwtTokenService<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TAuthenticationService : KirelSmsAuthenticationService<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TAuthorizedUserService : KirelAuthorizedUserService<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim, TAuthorizedUserDto, TAuthorizedUserUpdateDto>
    where TRoleClaim : KirelIdentityRoleClaim<TKey>
    where TUserClaim : KirelIdentityUserClaim<TKey>
{
    /// <summary>
    /// Authentication service
    /// </summary>
    protected readonly TAuthenticationService SmsAuthenticationService;

    /// <summary>
    /// Provider to get the current authorized user
    /// </summary>
    protected readonly TAuthorizedUserService AuthorizedUserService;

    /// <summary>
    /// Service for generating JWT token and JWT Refresh tokens for the user
    /// </summary>
    protected readonly TTokenService TokenService;

    /// <summary>
    /// KirelAuthenticationController constructor
    /// </summary>
    /// <param name="authService"> Authentication service </param>
    /// <param name="tokenService"> Service for generating JWT dto </param>
    /// <param name="authorizedUserProvider"> </param>
    public KirelJwtSmsAuthenticationController(TAuthenticationService authService, TTokenService tokenService,
        TAuthorizedUserService authorizedUserProvider)
    {
        TokenService = tokenService;
        SmsAuthenticationService = authService;
        AuthorizedUserService = authorizedUserProvider;
    }
    
    /// <summary>
    /// Get single-use 4-digit code to the phone number
    /// </summary>
    /// <param name="phoneNumber">User phone number</param>
    [HttpPost("passwordless/phone")]
    public virtual async Task<ActionResult> GetPhoneCode([Required] string phoneNumber)
    {
        await SmsAuthenticationService.SendCodeBySms(phoneNumber);
        return NoContent();
    }

    /// <summary>
    /// Getting Jwt token
    /// </summary>
    /// <param name="phoneNumber"> User phone number </param>
    /// <param name="token"> User 4-digit token from sms</param>
    /// <returns> Token dto </returns>
    [HttpGet("passwordless/phone")]
    public virtual async Task<ActionResult<JwtTokenDto>> GetJwtToken(
        [Required] string phoneNumber,
        [Required] string token)
    {
        var user = await SmsAuthenticationService.LoginByCode(phoneNumber, token);
        return Ok(await TokenService.GenerateJwtTokenDto(user));
    }
}