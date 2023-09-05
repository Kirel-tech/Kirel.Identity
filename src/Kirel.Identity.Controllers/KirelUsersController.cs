using Kirel.DTO;
using Kirel.Identity.Core.Models;
using Kirel.Identity.Core.Services;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Controllers;

/// <summary>
/// Controller for user management
/// </summary>
/// <typeparam name="TUserService"> User management service type </typeparam>
/// <typeparam name="TKey"> Id type </typeparam>
/// <typeparam name="TUser"> User type. Must be a descendant of the KirelIdentityUser class </typeparam>
/// <typeparam name="TRole"> Role type. Must be a descendant of the KirelIdentityRole class </typeparam>
/// <typeparam name="TUserDto"> User dto type </typeparam>
/// <typeparam name="TUserCreateDto"> User create dto type. Must be a descendant of the UserCreateDto class </typeparam>
/// <typeparam name="TUserUpdateDto"> User update dto type. Must be a descendant of the UserUpdateDto class </typeparam>
/// <typeparam name="TClaimDto"> Claim dto type. Must be a descendant of the KirelClaimDto class </typeparam>
/// <typeparam name="TClaimCreateDto"> Claim create dto type. Must be a descendant of the KirelClaimCreateDto class </typeparam>
/// <typeparam name="TClaimUpdateDto"> Claim update dto type. Must be a descendant of the KirelClaimUpdateDto class </typeparam>
/// <typeparam name="TUserRole"> The user role entity type </typeparam>
public class KirelUsersController<TUserService, TKey, TUser, TRole, TUserRole, TUserDto, TUserCreateDto, TUserUpdateDto, TClaimDto,
    TClaimCreateDto, TClaimUpdateDto> : Controller
    where TUserService : KirelUserService<TKey, TUser, TRole, TUserRole, TUserDto, TUserCreateDto, TUserUpdateDto, TClaimDto,
        TClaimCreateDto, TClaimUpdateDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole>
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
    /// <param name="service"> User management service </param>
    public KirelUsersController(TUserService service)
    {
        Service = service;
    }

    /// <summary>
    /// Create user
    /// </summary>
    /// <param name="createDto"> User create dto </param>
    /// <returns> User dto </returns>
    [HttpPost]
    [Authorize(Roles = "Admin, Microservice", AuthenticationSchemes = "Bearer, APIKey")]
    public virtual async Task<ActionResult<TUserDto>> Create([FromBody] TUserCreateDto createDto)
    {
        var dto = await Service.CreateUser(createDto);
        return Ok(dto);
    }

    /// <summary>
    /// Update user
    /// </summary>
    /// <param name="updateDto"> User update dto </param>
    /// <param name="id"> User id </param>
    /// <returns> User dto </returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, Microservice", AuthenticationSchemes = "Bearer, APIKey")]
    public virtual async Task<ActionResult<TUserDto>> Update([FromBody] TUserUpdateDto updateDto, TKey id)
    {
        var dto = await Service.UpdateUser(id, updateDto);
        return Ok(dto);
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="id"> User id </param>
    /// <returns> User dto </returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Microservice", AuthenticationSchemes = "Bearer, APIKey")]
    public virtual async Task<ActionResult<TUserDto>> GetById(TKey id)
    {
        var result = await Service.GetById(id);
        return Ok(result);
    }

    /// <summary>
    /// Get users list
    /// </summary>
    /// <param name="pageNumber"> The number of the displayed page </param>
    /// <param name="pageSize"> Number of items per page </param>
    /// <param name="orderBy"> Name of the sorting field </param>
    /// <param name="orderDirection"> Order direction </param>
    /// <param name="search"> Search string parameter </param>
    /// <param name="roleIds"></param>
    /// <returns> Paginated result with list of users dto </returns>
    [HttpGet]
    [Authorize(Roles = "Admin, Microservice", AuthenticationSchemes = "Bearer, APIKey")]
    public virtual async Task<PaginatedResult<List<TUserDto>>> GetList([FromQuery] int pageNumber = 0, int pageSize = 0,
        string orderBy = "", string orderDirection = "asc", string search = "", [FromQuery] IEnumerable<TKey>? roleIds = null)
    {
        var directionEnum = SortDirection.Asc;
        if (orderDirection == "desc") directionEnum = SortDirection.Desc;
        return await Service.GetUsersList(pageNumber, pageSize, search, orderBy, directionEnum, roleIds);
    }
}