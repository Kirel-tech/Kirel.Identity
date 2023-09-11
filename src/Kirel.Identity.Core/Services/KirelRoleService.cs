using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Kirel.DTO;
using Kirel.Identity.Core.Models;
using Kirel.Identity.DTOs;
using Kirel.Identity.Exceptions;
using Kirel.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Kirel.Identity.Core.Services;

/// <summary>
/// Service for management roles and their claims
/// </summary>
/// <typeparam name="TKey"> Id type </typeparam>
/// <typeparam name="TRole"> Role type </typeparam>
/// <typeparam name="TRoleDto"> Role response dto </typeparam>
/// <typeparam name="TRoleCreateDto"> Role create dto. Must be a descendant of the RoleCreateDto class </typeparam>
/// <typeparam name="TRoleUpdateDto"> Role update dto. Must be a descendant of the RoleUpdateDto class </typeparam>
/// <typeparam name="TClaimDto"> Claim dto. Must be a descendant of the KirelClaimDto class </typeparam>
/// <typeparam name="TClaimCreateDto"> Claim create dto. Must be a descendant of the KirelClaimCreateDto class </typeparam>
/// <typeparam name="TClaimUpdateDto"> Claim update dto. Must be a descendant of the KirelClaimUpdateDto class </typeparam>
/// <typeparam name="TUser"> User entity type. </typeparam>
/// <typeparam name="TUserRole"> User role entity type. </typeparam>
/// <typeparam name="TUserClaim"> User claim type. </typeparam>
/// <typeparam name="TRoleClaim"> Role claim type. </typeparam>
public class KirelRoleService<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim, TRoleDto, TRoleCreateDto, TRoleUpdateDto, TClaimDto, TClaimCreateDto,
    TClaimUpdateDto>
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
    /// AutoMapper instance
    /// </summary>
    protected readonly IMapper Mapper;

    /// <summary>
    /// Identity role manager
    /// </summary>
    protected readonly RoleManager<TRole> RoleManager;

    /// <summary>
    /// Roles queryable list
    /// </summary>
    protected virtual IQueryable<TRole> Roles { get => RoleManager.Roles.AsQueryable(); }

    /// <summary>
    /// KirelRoleService constructor
    /// </summary>
    /// <param name="roleManager"> Identity role manager </param>
    /// <param name="mapper"> AutoMapper instance </param>
    public KirelRoleService(RoleManager<TRole> roleManager, IMapper mapper)
    {
        RoleManager = roleManager;
        Mapper = mapper;
    }

    /// <summary>
    /// Creates a new role
    /// </summary>
    /// <param name="createDto"> Role create dto </param>
    /// <exception cref="KirelAlreadyExistException"> If role already exists </exception>
    /// <exception cref="KirelIdentityStoreException"> If role manager fails to create role </exception>
    public virtual async Task<TRoleDto> CreateRole(TRoleCreateDto createDto)
    {
        if (await RoleManager.RoleExistsAsync(createDto.Name))
            throw new KirelAlreadyExistException($"Role with given name {createDto.Name} already exist");
        var role = Mapper.Map<TRole>(createDto);
        var result = await RoleManager.CreateAsync(role);
        if (!result.Succeeded)
            throw new KirelIdentityStoreException("Failed to create new role");
        var newRoleDto = Mapper.Map<TRoleDto>(role);
        return newRoleDto;
    }

    /// <summary>
    /// Updates role by id
    /// </summary>
    /// <param name="roleId"> Role id </param>
    /// <param name="updateDto"> Role update dto </param>
    /// <returns> Updated role dto </returns>
    /// <exception cref="KirelNotFoundException"> If role was not found </exception>
    /// <exception cref="KirelAlreadyExistException"> If role with given name already exists </exception>
    public virtual async Task<TRoleDto> UpdateRole(TKey roleId, TRoleUpdateDto updateDto)
    {
        var role = await Roles.FirstOrDefaultAsync(r => r.Id.Equals(roleId));
        if (role == null)
            throw new KirelNotFoundException($"Role with specified id {roleId} was not found");
        Mapper.Map(updateDto, role);
        await RoleManager.UpdateAsync(role);
        var returnDto = Mapper.Map<TRoleDto>(role);
        return returnDto;
    }

    /// <summary>
    /// Get role by id
    /// </summary>
    /// <param name="roleId"> Role id </param>
    /// <returns> Role dto </returns>
    /// <exception cref="KirelNotFoundException"> If role was not found </exception>
    public virtual async Task<TRoleDto> GetRole(TKey roleId)
    {
        var role = await Roles.FirstOrDefaultAsync(r => r.Id.Equals(roleId));
        if (role == null)
            throw new KirelNotFoundException($"Role with specified id {roleId} was not found");
        return Mapper.Map<TRoleDto>(role);
    }

    /// <summary>
    /// Gets a list of roles with search and pagination
    /// </summary>
    /// <param name="page"> Page number </param>
    /// <param name="pageSize"> Page size </param>
    /// <param name="search"> Search keyword </param>
    /// <param name="orderBy"> Field name to order by </param>
    /// <param name="orderDirection"> Ascending or descending order direction </param>
    /// <returns> List of roles dto with pagination </returns>
    /// <exception cref="ArgumentOutOfRangeException"> If passed wrong sort direction </exception>
    public virtual async Task<PaginatedResult<List<TRoleDto>>> GetRolesList(int page, int pageSize, string search,
        string orderBy, SortDirection orderDirection)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;
        Expression<Func<TRole, bool>>? searchExpression = _ => true;
        Expression<Func<TRole, bool>>? rolesIdsExpression = _ => true;
        Func<IQueryable<TRole>, IOrderedQueryable<TRole>>? orderByFunc = null;

        if (!string.IsNullOrEmpty(search))
            searchExpression = PredicateBuilder.PredicateSearchInAllFields<TRole>(search, true);
        
        if (!string.IsNullOrEmpty(orderBy)) 
            orderByFunc = ServiceHelper.GenerateOrderingMethod<TRole>(orderBy, orderDirection);

        var finalExpression = PredicateBuilder.And(searchExpression, rolesIdsExpression);
        
        return await GetRolesList(page, pageSize, finalExpression, orderByFunc);
    }
    internal virtual async Task<PaginatedResult<List<TRoleDto>>> GetRolesList(int page, int pageSize, 
        Expression<Func<TRole, bool>>? filterBy,
        Func<IQueryable<TRole>, IOrderedQueryable<TRole>>? orderBy)
    {
        var appRoles = RoleManager.Roles.AsQueryable();
        appRoles = orderBy == null ? appRoles.OrderByDescending(u => u.Name) : orderBy(appRoles);
        
        if (filterBy != null) appRoles = appRoles.Where(filterBy);
        var count = await appRoles.CountAsync();
        
        appRoles = appRoles.Skip((page - 1) * pageSize).Take(pageSize);
        
        var pagination = Pagination.Generate(page, pageSize, count);
        return new PaginatedResult<List<TRoleDto>>
        {
            Pagination = pagination,
            Data = Mapper.Map<List<TRoleDto>>(appRoles)
        };
    }
}