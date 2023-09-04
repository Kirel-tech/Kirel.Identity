using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Kirel.Identity.Core.Models;
using Kirel.Identity.Core.Services;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Controllers;

/// <summary>
/// Controller for authorized user account management
/// </summary>
/// <typeparam name="TAuthorizedUserService">
/// Authorized user service. Must be a descendant of the
/// KirelAuthorizedUserService class
/// </typeparam>
/// <typeparam name="TKey"> User key type </typeparam>
/// <typeparam name="TUser"> User type </typeparam>
/// <typeparam name="TAuthorizedUserDto"> Authorized user dto </typeparam>
/// <typeparam name="TAuthorizedUserUpdateDto"> Authorized user update dto </typeparam>
/// <typeparam name="TRole"> The role entity type </typeparam>
/// <typeparam name="TUserRole"> The user role entity type </typeparam>
public class KirelAuthorizedUserController<TAuthorizedUserService, TKey, TUser, TRole, TUserRole, TAuthorizedUserDto,
    TAuthorizedUserUpdateDto> : Controller
    where TAuthorizedUserService : KirelAuthorizedUserService<TKey, TUser, TRole, TUserRole, TAuthorizedUserDto, TAuthorizedUserUpdateDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole>
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
    /// <param name="service"> Service for authorized user account management </param>
    /// <param name="mapper"> AutoMapper instance </param>
    public KirelAuthorizedUserController(TAuthorizedUserService service, IMapper mapper)
    {
        AuthorizedUserService = service;
        Mapper = mapper;
    }

    /// <summary>
    /// Gets authorized user account info
    /// </summary>
    /// <returns> Authorized user dto </returns>
    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public virtual async Task<ActionResult<TAuthorizedUserDto>> GetInfo()
    {
        var result = await AuthorizedUserService.GetDto();
        return Ok(result);
    }

    /// <summary>
    /// Update authorized user account info
    /// </summary>
    /// <param name="updateDto"> Authorized user update dto </param>
    /// <returns> Authorized user dto </returns>
    [HttpPut]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public virtual async Task<ActionResult<TAuthorizedUserDto>> Update([FromBody] TAuthorizedUserUpdateDto updateDto)
    {
        var dto = await AuthorizedUserService.Update(updateDto);
        return Ok(dto);
    }

    /// <summary>
    /// Change authorized user password
    /// </summary>
    /// <param name="currentPassword"> Current user password </param>
    /// <param name="newPassword"> New user password </param>
    [HttpPut("password")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public virtual async Task<ActionResult> ChangePassword(
        [Required] string currentPassword,
        [Required] string newPassword)
    {
        await AuthorizedUserService.ChangeUserPassword(currentPassword, newPassword);
        return NoContent();
    }
}