using System.ComponentModel.DataAnnotations;
using Kirel.Identity.Core.Models;
using Kirel.Identity.Core.Services;
using Kirel.Identity.DTOs;
using Kirel.Identity.Jwt.Core.Services;
using Kirel.Identity.Jwt.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Jwt.Controllers;

/// <summary>
/// Controller for JWT token management
/// </summary>
/// <typeparam name="TTokenService"> Authentication service. Must be a descendant of the KirelAuthenticationService class </typeparam>
/// <typeparam name="TAuthenticationService">
/// Authentication service. Must be a descendant of the
/// KirelAuthenticationService class
/// </typeparam>
/// <typeparam name="TAuthorizedUserService">
/// Authorized user service. Must be a descendant of the
/// KirelAuthorizedUserService class
/// </typeparam>
/// <typeparam name="TKey"> User key type </typeparam>
/// <typeparam name="TUser"> User type </typeparam>
/// <typeparam name="TRole"> Role type </typeparam>
/// <typeparam name="TAuthorizedUserDto"> </typeparam>
/// <typeparam name="TAuthorizedUserUpdateDto"> </typeparam>
/// <typeparam name="TUserRole"> The user role entity type </typeparam>
/// <typeparam name="TEmailAuthenticationService"> Email authentication service </typeparam>
public class KirelJwtAuthenticationController<TTokenService, TAuthenticationService, TAuthorizedUserService, TEmailAuthenticationService,
    TKey, TUser, TRole, TUserRole, TAuthorizedUserDto, TAuthorizedUserUpdateDto> : Controller
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole>
    where TAuthorizedUserDto : KirelAuthorizedUserDto
    where TAuthorizedUserUpdateDto : KirelAuthorizedUserUpdateDto
    where TTokenService : KirelJwtTokenService<TKey, TUser, TRole, TUserRole>
    where TAuthenticationService : KirelAuthenticationService<TKey, TUser, TRole, TUserRole>
    where TAuthorizedUserService : KirelAuthorizedUserService<TKey, TUser, TRole, TUserRole, TAuthorizedUserDto,
        TAuthorizedUserUpdateDto>
    where TEmailAuthenticationService : KirelEmailAuthenticationService<TKey, TUser, TRole, TUserRole>
{
    /// <summary>
    /// Authentication service
    /// </summary>
    protected readonly TAuthenticationService AuthenticationService;

    /// <summary>
    /// Provider to get the current authorized user
    /// </summary>
    protected readonly TAuthorizedUserService AuthorizedUserService;

    /// <summary>
    /// Service for generating JWT token and JWT Refresh tokens for the user
    /// </summary>
    protected readonly TTokenService TokenService;

    /// <summary>
    /// Email authentication service 
    /// </summary>
    protected readonly TEmailAuthenticationService EmailAuthenticationService;

    /// <summary>
    /// KirelAuthenticationController constructor
    /// </summary>
    /// <param name="authService"> Authentication service </param>
    /// <param name="tokenService"> Service for generating JWT dto </param>
    /// <param name="authorizedUserProvider"> </param>
    /// <param name="emailAuthenticationService">Email authentication service  </param>
    public KirelJwtAuthenticationController(TAuthenticationService authService, TTokenService tokenService,
        TAuthorizedUserService authorizedUserProvider, TEmailAuthenticationService emailAuthenticationService)
    {
        TokenService = tokenService;
        AuthenticationService = authService;
        AuthorizedUserService = authorizedUserProvider;
        EmailAuthenticationService = emailAuthenticationService;
    }

    /// <summary>
    /// Get Jwt token
    /// </summary>
    /// <param name="login"> User login </param>
    /// <param name="password"> User password </param>
    /// <returns> Token dto </returns>
    [HttpGet]
    [Produces("application/json", "application/problem+json")]
    [ProducesResponseType(typeof(ProblemDetails), 401)]
    [ProducesResponseType(typeof(JwtTokenDto), 200)]
    public virtual async Task<ActionResult<JwtTokenDto>> GetJwtToken(
        [Required] string login,
        [Required] string password)
    {
        var user = await AuthenticationService.LoginByPassword(login, password);
        return Ok(await TokenService.GenerateJwtTokenDto(user));
    }

    /// <summary>
    /// Get JWT token by refresh token
    /// </summary>
    /// <returns> Token dto </returns>
    [HttpPut]
    [Authorize(Roles = "RefreshToken", AuthenticationSchemes = "Bearer")]
    public virtual async Task<ActionResult<JwtTokenDto>> RefreshToken()
    {
        var user = await AuthorizedUserService.Get();
        return Ok(await TokenService.GenerateJwtTokenDto(user));
    }

    /// <summary>
    /// Method for user login using an email address and a token.
    /// </summary>
    /// <param name="email"> User's email address. </param>
    /// <param name="token"> Token for user login. </param>
    /// <returns> The result of the login operation, including the token. </returns>
    [HttpGet("email/login")]
    public virtual async Task<ActionResult<JwtTokenDto>> Login(
        [Required] string email,
        [Required] string token)
    {
        var user = await EmailAuthenticationService.LoginByToken(email, token);
        return Ok(await TokenService.GenerateJwtTokenDto(user));
    }

    /// <summary>
    /// Method for sending a login token to an email address.
    /// </summary>
    /// <param name="email"> Email address to which the login token will be sent. </param>
    /// <returns> The result of the token sending operation. </returns>
    [HttpPost("email/send")]
    public virtual async Task<ActionResult> SendMailToken([Required] string email)
    {
        await EmailAuthenticationService.SendTokenOnMail(email);
        return Ok();
    }
}