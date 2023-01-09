using AutoMapper;
using Kirel.Identity.Core.DTOs;
using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Services;
using Microsoft.AspNetCore.Identity;
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
    where TUser : IdentityUser<TKey>, IKirelUser<TKey>
    where TAuthorizedUserDto : KirelAuthorizedUserDto
    where TAuthorizedUserUpdateDto : KirelAuthorizedUserUpdateDto
{
    /// <summary>
    /// Authorized user service. Must be a descendant of the KirelAuthorizedUserService class
    /// </summary>
    protected readonly TAuthorizedUserService Service;
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
        Service = service;
        Mapper = mapper;
    }

    private TKey GetAuthorizedUserId()
    {
        var authorizedUserId = User.Claims.First(claim => claim.Type == "uid").Value;
        if (typeof(TKey) == typeof(Guid))
        {
            object guidId = Guid.Parse(authorizedUserId);
            return (TKey)guidId;
        }
        if (typeof(TKey) == typeof(int))
        {
            var intId = int.Parse(authorizedUserId);
            return (TKey)(object)intId;
        }
        if (typeof(TKey) == typeof(string))
        {
            return (TKey)(object)authorizedUserId;
        }
        throw new Exception("Unsupported TKey type");
    }
    
    /// <summary>
    /// Gets authorized user account info
    /// </summary>
    /// <returns>Authorized user dto</returns>
    [HttpGet]
    public virtual async Task<ActionResult<TAuthorizedUserDto>> GetInfo()
    {
        var result = await Service.GetById(GetAuthorizedUserId());
        if (result != null) return Ok(result);
        return NotFound();
    }
    
    /// <summary>
    /// Update authorized user account info
    /// </summary>
    /// <param name="authUpdateDto">Authorized user update dto</param>
    /// <returns>Authorized user dto</returns>
    [HttpPut]
    public virtual async Task<ActionResult<KirelAuthorizedUserDto>> Update([FromBody] TAuthorizedUserUpdateDto authUpdateDto)
    {
        var authorizedUserId = GetAuthorizedUserId();
        var user = await Service.GetById(authorizedUserId);
        var updateDto = Mapper.Map<TAuthorizedUserUpdateDto>(user);
        Mapper.Map(authUpdateDto, updateDto);
        var dto =  await Service.UpdateUser(authorizedUserId, updateDto);
        if (dto != null) return Ok(dto);
        return BadRequest();
    }
    
    /// <summary>
    /// Change authorized user password
    /// </summary>
    /// <param name="currentPassword">Current user password</param>
    /// <param name="newPassword">New user password</param>
    [HttpPut("password")]
    public virtual async Task<ActionResult> ChangePassword(string currentPassword, string newPassword)
    {
        await Service.ChangeUserPassword(GetAuthorizedUserId(), currentPassword, newPassword);
        return NoContent();
    }
}