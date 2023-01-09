using Kirel.DTO;
using Kirel.Identity.Core.DTOs;
using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Controllers;

/// <summary>
/// Controller for user management
/// </summary>
/// <typeparam name="TUserService">User management service</typeparam>
/// <typeparam name="TKey">Id type</typeparam>
/// <typeparam name="TUser">User type. Must be an implementation of the IApplicationUser interface</typeparam>
/// <typeparam name="TUserDto">User dto</typeparam>
/// <typeparam name="TUserCreateDto">User create dto. Must be a descendant of the UserCreateDto class</typeparam>
/// <typeparam name="TUserUpdateDto">User update dto. Must be a descendant of the UserUpdateDto class</typeparam>
/// <typeparam name="TClaimDto">Claim dto. Must be a descendant of the KirelClaimDto class</typeparam>
/// <typeparam name="TClaimCreateDto">Claim create dto. Must be a descendant of the KirelClaimCreateDto class</typeparam>
/// <typeparam name="TClaimUpdateDto">Claim update dto. Must be a descendant of the KirelClaimUpdateDto class</typeparam>
public class KirelUsersController<TUserService,TKey,TUser,TUserDto,TUserCreateDto,TUserUpdateDto,TClaimDto,TClaimCreateDto,TClaimUpdateDto> : Controller
    where TUserService : KirelUserService<TKey,TUser,TUserDto,TUserCreateDto,TUserUpdateDto,TClaimDto,TClaimCreateDto,TClaimUpdateDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey> 
    where TUser : IdentityUser<TKey>, IKirelUser<TKey> 
    where TUserDto : KirelUserDto<TKey, TKey, TClaimDto>
    where TUserCreateDto : KirelUserCreateDto<TKey, TClaimCreateDto>
    where TUserUpdateDto : KirelUserUpdateDto<TKey, TClaimUpdateDto>
    where TClaimDto : KirelClaimDto
    where TClaimCreateDto : KirelClaimCreateDto
    where TClaimUpdateDto : KirelClaimUpdateDto
{
    /// <summary>
    /// User management service
    /// </summary>
    protected readonly TUserService Service;
    
    /// <summary>
    /// KirelUsersController constructor
    /// </summary>
    /// <param name="service">User management service</param>
    public KirelUsersController(TUserService service)
    {
        Service = service;
    }
    
    /// <summary>
    /// Create new user
    /// </summary>
    /// <param name="createDto">User create dto</param>
    /// <returns>User dto</returns>
    [HttpPost]
    public virtual async Task<ActionResult<TUserDto>> Create([FromBody] TUserCreateDto createDto)
    {
        var dto = await Service.CreateUser(createDto);
        if (dto != null) return Ok(dto);
        return BadRequest();
    }
    
    /// <summary>
    /// Update user
    /// </summary>
    /// <param name="updateDto">User update dto</param>
    /// <param name="id">User id</param>
    /// <returns>User dto</returns>
    [HttpPut("{id}")]
    public virtual async Task<ActionResult<TUserDto>> Update([FromBody] TUserUpdateDto updateDto, TKey id)
    {
        var dto =  await Service.UpdateUser(id, updateDto);
        if (dto != null) return Ok(dto);
        return BadRequest();
    }

    /// <summary>
    /// Gets user by id
    /// </summary>
    /// <param name="id">User id</param>
    /// <returns>User dto</returns>
    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TUserDto>> GetById(TKey id)
    {
        var result = await Service.GetById(id);
        if (result != null) return Ok(result);
        return NotFound();
    }
    
    /// <summary>
    /// Gets user paginated list
    /// </summary>
    /// <param name="pageNumber">The number of the displayed page</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="orderBy">Name of the sorting field</param>
    /// <param name="orderDirection">Order direction</param>
    /// <param name="search">Search string parameter</param>
    /// <returns>Paginated result with list of users dto</returns>
    [HttpGet]
    public virtual async Task<PaginatedResult<List<TUserDto>>> GetList([FromQuery] int pageNumber = 0, int pageSize = 0,
        string orderBy = "", string orderDirection = "asc", string search = "")
    {
        var directionEnum = SortDirection.Asc;
        if (orderDirection == "desc")
        {
            directionEnum = SortDirection.Desc;
        }
        return await Service.GetUsersList(pageNumber, pageSize, search, orderBy, directionEnum);
    }
}