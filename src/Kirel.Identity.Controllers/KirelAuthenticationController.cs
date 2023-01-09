using System.ComponentModel.DataAnnotations;
using Kirel.Identity.DTOs;
using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Controllers;

/// <summary>
/// Controller for JWT token management
/// </summary>
/// <typeparam name="TAuthenticationService">Authentication service. Must be a descendant of the KirelAuthenticationService class</typeparam>
/// <typeparam name="TKey">User key type</typeparam>
/// <typeparam name="TUser">User type</typeparam>
/// <typeparam name="TRegisterDto">User register dto type</typeparam>
public class KirelAuthenticationController<TAuthenticationService, TKey, TUser, TRegisterDto> : Controller
    where TAuthenticationService : KirelAuthenticationService<TKey, TUser, TRegisterDto> 
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey> 
    where TUser : IdentityUser<TKey>, IKirelUser<TKey> 
    where TRegisterDto : KirelUserRegisterDto
{
    /// <summary>
    /// Authentication service
    /// </summary>
    protected readonly TAuthenticationService AuthenticationService;
    /// <summary>
    /// KirelAuthenticationController constructor
    /// </summary>
    /// <param name="authService">Authentication service</param>
    public KirelAuthenticationController(TAuthenticationService authService)
    {
        AuthenticationService = authService;
    }

    /// <summary>
    /// Getting Jwt token
    /// </summary>
    /// <param name="login">User login</param>
    /// <param name="password">User password</param>
    /// <returns>Token dto</returns>
    [HttpGet]
    public virtual async Task<ActionResult<JwtTokenDto>> GetJwtToken(
        [Required] string login,
        [Required] string password)
    {
        var token = await AuthenticationService.GetJwtToken(login, password);
        if (token == null) return BadRequest();
        return Ok(token);
    }
    
    /// <summary>
    /// Getting new JWT token via refresh token
    /// </summary>
    /// <returns>Token dto</returns>
    [HttpPut]
    [Authorize(Roles = "RefreshToken")]
    public virtual async Task<ActionResult<JwtTokenDto>> RefreshToken()
    {
        var login = HttpContext.User.Identity?.Name;
        if (login == null) 
            return BadRequest();
        var token = await AuthenticationService.RefreshToken(login);
        if (token == null) 
            return BadRequest();
        return Ok(token);
    }
}