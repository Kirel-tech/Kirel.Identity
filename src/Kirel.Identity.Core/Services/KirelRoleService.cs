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
public class KirelRoleService<TKey, TRole, TUser, TUserRole, TRoleDto, TRoleCreateDto, TRoleUpdateDto, TClaimDto, TClaimCreateDto,
    TClaimUpdateDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole>
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
    /// KirelRoleService constructor
    /// </summary>
    /// <param name="roleManager"> Identity role manager </param>
    /// <param name="mapper"> AutoMapper instance </param>
    public KirelRoleService(RoleManager<TRole> roleManager, IMapper mapper)
    {
        RoleManager = roleManager;
        Mapper = mapper;
    }

    private async Task SyncRoleClaims(TKey roleId, ICollection<TClaimUpdateDto> claims)
    {
        var roleClaims = await GetRoleClaims(roleId);
        foreach (var claim in claims.Select(dtoClaim => new Claim(dtoClaim.Type, dtoClaim.Value)))
            if (!roleClaims.Contains(claim))
                await AddClaimToRole(claim, roleId);
        foreach (var claim in roleClaims)
        {
            var dtoClaim = Mapper.Map<TClaimUpdateDto>(claim);
            if (!claims.Contains(dtoClaim)) await DeleteClaimFromRole(claim, roleId);
        }
    }

    /// <summary>
    /// Adds a claim to the specified role
    /// </summary>
    /// <param name="claim"> Claim </param>
    /// <param name="roleId"> Role id </param>
    /// <exception cref="KirelNotFoundException"> If role was not found </exception>
    private async Task AddClaimToRole(Claim claim, TKey roleId)
    {
        var role = await RoleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
            throw new KirelNotFoundException($"Role with given id {roleId} was not found");
        await RoleManager.AddClaimAsync(role, claim);
    }

    /// <summary>
    /// Deletes claim from role
    /// </summary>
    /// <param name="claim"> Claim to delete </param>
    /// <param name="roleId"> Role id </param>
    /// <exception cref="KirelNotFoundException"> If role was not found </exception>
    private async Task DeleteClaimFromRole(Claim claim, TKey roleId)
    {
        var role = await RoleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
            throw new KirelNotFoundException($"Role with given id {roleId} was not found");
        await RoleManager.RemoveClaimAsync(role, claim);
    }

    /// <summary>
    /// Get claims of specified role
    /// </summary>
    /// <param name="roleId"> Role id </param>
    /// <returns> List of role claims </returns>
    /// <exception cref="KirelNotFoundException"> If role with given name was not found </exception>
    public virtual async Task<IList<Claim>> GetRoleClaims(TKey roleId)
    {
        var role = await RoleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
            throw new KirelNotFoundException($"Role with given id {roleId} was not found");
        return await RoleManager.GetClaimsAsync(role);
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
        var newRole = await RoleManager.FindByNameAsync(createDto.Name);
        var claims = Mapper.Map<List<Claim>>(createDto.Claims);
        foreach (var claim in claims) await AddClaimToRole(claim, newRole.Id);
        var newRoleDto = Mapper.Map<TRoleDto>(newRole);
        newRoleDto.Claims = Mapper.Map<List<TClaimDto>>(await GetRoleClaims(newRole.Id));
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
        var role = await RoleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
            throw new KirelNotFoundException($"Role with specified id {roleId} was not found");
        Mapper.Map(updateDto, role);
        await RoleManager.UpdateAsync(role);
        await SyncRoleClaims(roleId, updateDto.Claims);
        var returnDto = Mapper.Map<TRoleDto>(role);
        returnDto.Claims = Mapper.Map<List<TClaimDto>>(await GetRoleClaims(roleId));
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
        var role = await RoleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
            throw new KirelNotFoundException($"Role with specified id {roleId} was not found");
        var roleDto = Mapper.Map<TRoleDto>(role);
        roleDto.Claims = Mapper.Map<List<TClaimDto>>((await GetRoleClaims(roleId)).ToList());
        return roleDto;
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
        page = page < 1 ? 0 : page;
        pageSize = pageSize < 1 ? 0 : pageSize;
        Expression<Func<TRole, bool>> filterExpression = null;
        Expression<Func<TRole, object>> orderExpression = null;

        if (!string.IsNullOrEmpty(search))
            filterExpression = PredicateBuilder.PredicateSearchInAllFields<TRole>(search, true);

        var appRoles = RoleManager.Roles;
        if (!string.IsNullOrEmpty(orderBy)) orderExpression = PredicateBuilder.ToLambda<TRole>(orderBy);

        if (orderExpression != null)
            appRoles = orderDirection switch
            {
                SortDirection.Asc => appRoles.OrderBy(orderExpression),
                SortDirection.Desc => appRoles.OrderByDescending(orderExpression),
                _ => throw new ArgumentOutOfRangeException(nameof(orderDirection), orderDirection, null)
            };

        if (filterExpression != null) appRoles = appRoles.Where(filterExpression);

        var count = await appRoles.CountAsync();
        if (page > 0 && pageSize > 0)
            appRoles = appRoles.Skip((page - 1) * pageSize).Take(pageSize);

        var pagination = Pagination.Generate(page, pageSize, count);
        var data = Mapper.Map<List<TRoleDto>>(appRoles);
        foreach (var roleDto in data)
        {
            var roleClaims = await GetRoleClaims(roleDto.Id);
            roleDto.Claims = Mapper.Map<List<TClaimDto>>(roleClaims);
        }

        return new PaginatedResult<List<TRoleDto>>
        {
            Pagination = pagination,
            Data = data
        };
    }
}