using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Kirel.Identity.DTOs;
using Kirel.Identity.Core.Models;
using Kirel.Identity.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Controllers;

/// <summary> 
/// Controller for authorized user account management
/// </summary>
/// <typeparam name="TAuthorizedUserService">Authorized user service. Must be a descendant of the KirelAuthorizedUserService class</typeparam>
/// <typeparam name="TKey">User key type</typeparam>
/// <typeparam name="TUser">User type</typeparam>
/// <typeparam name="TAuthorizedUserDto">Authorized user dto</typeparam>
/// <typeparam name="TAuthorizedUserUpdateDto">Authorized user update dto</typeparam>
public class KirelAuthorizedUserController<TAuthorizedUserService, TKey, TUser, TAuthorizedUserDto, TAuthorizedUserUpdateDto> : Controller
    where TAuthorizedUserService : KirelAuthorizedUserService<TKey, TUser, TAuthorizedUserDto, TAuthorizedUserUpdateDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey>
    where TAuthorizedUserDto : KirelAuthorizedUserDto
    where TAuthorizedUserUpdateDto : KirelAuthorizedUserUpdateDto
{
    /// <summary>
    /// Authorized user service. Must be a descendant of the KirelAuthorizedUserService class
    /// </summary>
    protected readonly TAuthorizedUserService AuthorizedUserService;
    /// <summary>
    /// AutoMapper instance
    /// </summary>
    protected readonly IMapper Mapper;

    /// <summary>
    /// KirelAuthorizedUserController constructor
    /// </summary>
    /// <param name="service">Service for authorized user account management</param>
    /// <param name="mapper">AutoMapper instance</param>
    public KirelAuthorizedUserController(TAuthorizedUserService service, IMapper mapper)
    {
        AuthorizedUserService = service;
        Mapper = mapper;
    }

    /// <summary>
    /// Gets authorized user account info
    /// </summary>
    /// <returns>Authorized user dto</returns>
    [HttpGet]
    public virtual async Task<ActionResult<TAuthorizedUserDto>> GetInfo()
    {
        var result = await AuthorizedUserService.GetDto();
        if (result != null) return Ok(result);
        return NotFound();
    }
    
    /// <summary>
    /// Update authorized user account info
    /// </summary>
    /// <param name="updateDto">Authorized user update dto</param>
    /// <returns>Authorized user dto</returns>
    [HttpPut]
    public virtual async Task<ActionResult<TAuthorizedUserDto>> Update([FromBody] TAuthorizedUserUpdateDto updateDto)
    {
        var dto =  await AuthorizedUserService.Update(updateDto);
        if (dto != null) return Ok(dto);
        return BadRequest();
    }
    
    /// <summary>
    /// Change authorized user password
    /// </summary>
    /// <param name="currentPassword">Current user password</param>
    /// <param name="newPassword">New user password</param>
    [HttpPut("password")]
    public virtual async Task<ActionResult> ChangePassword(
        [Required] string currentPassword, 
        [Required] string newPassword)
    {
        await AuthorizedUserService.ChangeUserPassword(currentPassword, newPassword);
        return NoContent();
    }
}