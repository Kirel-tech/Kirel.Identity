﻿using Kirel.Identity.Core.Models;
using Kirel.Identity.Core.Services;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Controllers;

/// <summary>
/// Controller for user roles management
/// </summary>
/// <typeparam name="TRoleService"> Service for roles management type </typeparam>
/// <typeparam name="TKey"> Id type </typeparam>
/// <typeparam name="TRole"> Role type </typeparam>
/// <typeparam name="TRoleDto"> Role response dto type </typeparam>
/// <typeparam name="TRoleCreateDto"> Role create dto type. Must be a descendant of the RoleCreateDto class </typeparam>
/// <typeparam name="TRoleUpdateDto"> Role update dto type. Must be a descendant of the RoleUpdateDto class </typeparam>
/// <typeparam name="TClaimDto"> Claim dto type. Must be a descendant of the KirelClaimDto class </typeparam>
/// <typeparam name="TClaimCreateDto"> Claim create dto type. Must be a descendant of the KirelClaimCreateDto class </typeparam>
/// <typeparam name="TClaimUpdateDto"> Claim update dto type. Must be a descendant of the KirelClaimUpdateDto class </typeparam>
/// <typeparam name="TUser"> The user entity type </typeparam>
/// <typeparam name="TUserRole"> The user entity type </typeparam>
/// <typeparam name="TUserClaim"> User claim type. </typeparam>
/// <typeparam name="TRoleClaim"> Role claim type. </typeparam>
public class KirelRolesController<TRoleService, TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim, TRoleDto, TRoleCreateDto, TRoleUpdateDto, TClaimDto,
    TClaimCreateDto, TClaimUpdateDto> : Controller
    where TRoleService : KirelRoleService<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim, TRoleDto, TRoleCreateDto, TRoleUpdateDto, TClaimDto,
        TClaimCreateDto, TClaimUpdateDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
    where TRoleClaim : KirelIdentityRoleClaim<TKey>
    where TUserClaim : KirelIdentityUserClaim<TKey>
    where TRoleDto : KirelRoleDto<TKey, TClaimDto>
    where TRoleCreateDto : KirelRoleCreateDto<TClaimCreateDto>
    where TRoleUpdateDto : KirelRoleUpdateDto<TClaimUpdateDto>
    where TClaimDto : KirelClaimDto
    where TClaimCreateDto : KirelClaimCreateDto
    where TClaimUpdateDto : KirelClaimUpdateDto
{
    /// <summary>
    /// Service for roles management
    /// </summary>
    protected readonly TRoleService Service;

    /// <summary>
    /// RolesController constructor
    /// </summary>
    /// <param name="service"> Role management service </param>
    public KirelRolesController(TRoleService service)
    {
        Service = service;
    }

    /// <summary>
    /// Create role
    /// </summary>
    /// <param name="createDto"> Role create dto </param>
    /// <returns> Role dto </returns>
    [HttpPost]
    [Authorize(Policy = "role_create", AuthenticationSchemes = "Bearer, APIKey")]
    public virtual async Task<ActionResult<TRoleDto>> Create([FromBody] TRoleCreateDto createDto)
    {
        var role = await Service.CreateRole(createDto);
        return Ok(role);
    }

    /// <summary>
    /// Update role
    /// </summary>
    /// <param name="updateDto"> Role update dto </param>
    /// <param name="id"> Role id </param>
    /// <returns> Role dto </returns>
    [HttpPut("{id}")]
    [Authorize(Policy = "role_update", AuthenticationSchemes = "Bearer, APIKey")]
    public virtual async Task<ActionResult<TRoleDto>> Update([FromBody] TRoleUpdateDto updateDto, TKey id)
    {
        var dto = await Service.UpdateRole(id, updateDto);
        return Ok(dto);
    }

    /// <summary>
    /// Get role by ID
    /// </summary>
    /// <param name="id"> Role id </param>
    /// <returns> Role dto </returns>
    [HttpGet("{id}")]
    [Authorize(Policy = "role_read", AuthenticationSchemes = "Bearer, APIKey")]
    public virtual async Task<ActionResult<TRoleDto>> GetById(TKey id)
    {
        var result = await Service.GetRole(id);
        return Ok(result);
    }

    /// <summary>
    /// Get list of roles
    /// </summary>
    /// <param name="pageNumber"> The number of the displayed page </param>
    /// <param name="pageSize"> Number of items per page </param>
    /// <param name="orderBy"> Name of the sorting field </param>
    /// <param name="orderDirection"> Order direction </param>
    /// <param name="search"> Search string parameter </param>
    /// <returns> Paginated result with list of roles dto </returns>
    [HttpGet]
    [Authorize(Policy = "role_read", AuthenticationSchemes = "Bearer, APIKey")]
    public virtual async Task<PaginatedItemsDto<TRoleDto>> GetList([FromQuery] int pageNumber = 0, int pageSize = 0,
        string orderBy = "", string orderDirection = "asc", string search = "")
    {
        var directionEnum = SortDirectionDto.Asc;
        if (orderDirection == "desc") directionEnum = SortDirectionDto.Desc;
        return await Service.GetRolesList(pageNumber, pageSize, search, orderBy, directionEnum);
    }
}