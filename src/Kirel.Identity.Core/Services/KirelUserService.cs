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
/// Service for management users and their claims
/// </summary>
/// <typeparam name="TKey"> Id type </typeparam>
/// <typeparam name="TUser"> User type. Must be an implementation of the KirelIdentityUser class </typeparam>
/// <typeparam name="TRole"> Role type. Must be an implementation of the KirelIdentityRole class </typeparam>
/// <typeparam name="TUserDto"> User dto </typeparam>
/// <typeparam name="TUserCreateDto"> User create dto. Must be a descendant of the UserCreateDto class </typeparam>
/// <typeparam name="TUserUpdateDto"> User update dto. Must be a descendant of the UserUpdateDto class </typeparam>
/// <typeparam name="TClaimDto"> Claim dto. Must be a descendant of the KirelClaimDto class </typeparam>
/// <typeparam name="TClaimCreateDto"> Claim create dto. Must be a descendant of the KirelClaimCreateDto class </typeparam>
/// <typeparam name="TClaimUpdateDto"> Claim update dto. Must be a descendant of the KirelClaimUpdateDto class </typeparam>
/// <typeparam name="TUserRole"> User role entity type </typeparam>
public class KirelUserService<TKey, TUser, TRole, TUserRole, TUserDto, TUserCreateDto, TUserUpdateDto, TClaimDto, TClaimCreateDto,
    TClaimUpdateDto>
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
    /// AutoMapper instance
    /// </summary>
    protected readonly IMapper Mapper;

    /// <summary>
    /// Identity role manager
    /// </summary>
    protected readonly RoleManager<TRole> RoleManager;

    /// <summary>
    /// Identity user manager
    /// </summary>
    protected readonly UserManager<TUser> UserManager;

    /// <summary>
    /// KirelUserService constructor
    /// </summary>
    /// <param name="userManager"> Identity user manager </param>
    /// <param name="roleManager"> Identity role manager </param>
    /// <param name="mapper"> AutoMapper instance </param>
    public KirelUserService(UserManager<TUser> userManager, RoleManager<TRole> roleManager, IMapper mapper)
    {
        UserManager = userManager;
        RoleManager = roleManager;
        Mapper = mapper;
    }

    /// <summary>
    /// Deletes given claim from the user
    /// </summary>
    /// <param name="claim"> Claim to delete </param>
    /// <param name="userId"> User id </param>
    /// <exception cref="KirelNotFoundException"> If user or claim was not found </exception>
    private async Task DeleteClaimFromUser(Claim claim, TKey userId)
    {
        var user = await UserManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new KirelNotFoundException($"User with specified id {userId} was not found");

        var userClaims = await UserManager.GetClaimsAsync(user);
        if (!userClaims.Any(userClaim => userClaim.Type == claim.Type && userClaim.Value == claim.Value))
            throw new KirelNotFoundException($"This claim {claim.Type} was not found for the specified user {userId}");
        await UserManager.RemoveClaimAsync(user, claim);
    }

    /// <summary>
    /// Gets list of user claims
    /// </summary>
    /// <param name="userId"> User id </param>
    /// <returns> List of user claims </returns>
    /// <exception cref="KirelNotFoundException"> If user with given id was not found </exception>
    private async Task<IList<Claim>> GetUserClaims(TKey userId)
    {
        var user = await UserManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new KirelNotFoundException($"User with specified id {userId} was not found");
        return await UserManager.GetClaimsAsync(user);
    }

    private async Task<List<TKey>> GetUserRolesIds(TKey userId)
    {
        var user = await UserManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new KirelNotFoundException($"User with specified id {userId} was not found");
        var roles = await UserManager.GetRolesAsync(user);
        return await RoleManager.Roles.Where(r => roles.Contains(r.Name))
            .Select(r => r.Id).ToListAsync();
    }

    /// <summary>
    /// Adds new claim to the user
    /// </summary>
    /// <param name="claim"> Claim </param>
    /// <param name="userId"> User id </param>
    /// <exception cref="KirelNotFoundException"> If user with given id was not found </exception>
    private async Task AddClaimToUser(Claim claim, TKey userId)
    {
        var user = await UserManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new KirelNotFoundException($"User with specified id {userId} was not found");
        await UserManager.AddClaimAsync(user, claim);
    }

    private async Task SyncUserRoles(TKey userId, List<TKey> roles)
    {
        var user = await UserManager.FindByIdAsync(userId.ToString());
        var userRoles = await UserManager.GetRolesAsync(user);
        foreach (var roleId in roles)
        {
            var role = await RoleManager.FindByIdAsync(roleId.ToString());
            if (!userRoles.Contains(role.Name)) await AddUserToRole(userId, roleId);
        }

        foreach (var roleName in userRoles)
        {
            var role = await RoleManager.FindByNameAsync(roleName);
            if (!roles.Contains(role.Id)) await DeleteUserFromRole(userId, role.Id);
        }
    }

    private async Task SyncUserClaims(TKey userId, List<TClaimUpdateDto> dtoClaims)
    {
        var userClaims = await GetUserClaims(userId);
        var claimsFromDto = Mapper.Map<List<Claim>>(dtoClaims);
        foreach (var claim in claimsFromDto)
            if (!userClaims.Any(userClaim => userClaim.Type == claim.Type && userClaim.Value == claim.Value))
                await AddClaimToUser(claim, userId);
        foreach (var claim in userClaims)
            if (!claimsFromDto.Any(claimFromDto =>
                    claimFromDto.Type == claim.Type && claimFromDto.Value == claim.Value))
                await DeleteClaimFromUser(claim, userId);
    }

    /// <summary>
    /// Gets a user by id
    /// </summary>
    /// <param name="userId"> User id </param>
    /// <returns> User dto </returns>
    /// <exception cref="KirelNotFoundException"> If user with given id was not found </exception>
    public virtual async Task<TUserDto> GetById(TKey userId)
    {
        var user = await UserManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new KirelNotFoundException($"User with specified id {userId} was not found");
        var claims = await GetUserClaims(userId);
        var userDto = Mapper.Map<TUserDto>(user);
        userDto.Claims = Mapper.Map<List<TClaimDto>>(claims);
        userDto.Roles = await GetUserRolesIds(userId);
        return userDto;
    }

    /// <summary>
    /// Gets a list of users with search and pagination
    /// </summary>
    /// <param name="page"> Page number </param>
    /// <param name="pageSize"> Page size </param>
    /// <param name="search"> Search keyword </param>
    /// <param name="orderBy"> Field name to order by </param>
    /// <param name="orderDirection"> Ascending or descending order direction </param>
    /// <param name="roleIds"> Id's of the roles for users filtering </param>
    /// <returns> List of users dto with pagination </returns>
    /// <exception cref="ArgumentOutOfRangeException"> If passed wrong sort direction </exception>
    public virtual async Task<PaginatedResult<List<TUserDto>>> GetUsersList(int page, int pageSize, string search,
        string orderBy, SortDirection orderDirection, IEnumerable<TKey>? roleIds = null)
    {
        page = page < 1 ? 0 : page;
        pageSize = pageSize < 1 ? 0 : pageSize;
        Expression<Func<TUser, bool>>? searchExpression = _ => true;
        Expression<Func<TUser, bool>>? rolesIdsExpression = _ => true;
        Func<IQueryable<TUser>, IOrderedQueryable<TUser>>? orderByFunc = null;
        if (roleIds != null && roleIds.Any())
            rolesIdsExpression = u => u.UserRoles.Any(d => roleIds.Contains(d.RoleId));
        
        if (!string.IsNullOrEmpty(search))
            searchExpression = PredicateBuilder.PredicateSearchInAllFields<TUser>(search, true);
        
        if (!string.IsNullOrEmpty(orderBy)) 
            orderByFunc = ServiceHelper.GenerateOrderingMethod<TUser>(orderBy, orderDirection);

        var finalExpression = PredicateBuilder.And(searchExpression, rolesIdsExpression);
        
        return await GetUsersList(page, pageSize, finalExpression, orderByFunc);
    }
    
    /// <summary>
    /// Gets a list of users with search and pagination
    /// </summary>
    /// <param name="page"> Page number </param>
    /// <param name="pageSize"> Page size </param>
    /// <param name="filterBy"> Filter expression </param>
    /// <param name="orderBy"> Ordering method </param>
    /// <returns> List of users dto with pagination </returns>
    /// <exception cref="ArgumentOutOfRangeException"> If passed wrong sort direction </exception>
    internal virtual async Task<PaginatedResult<List<TUserDto>>> GetUsersList(int page, int pageSize, 
        Expression<Func<TUser, bool>>? filterBy,
        Func<IQueryable<TUser>, IOrderedQueryable<TUser>>? orderBy)
    {
        page = page < 1 ? 0 : page;
        pageSize = pageSize < 1 ? 0 : pageSize;

        var appUsers = UserManager.Users;
        appUsers = orderBy == null ? appUsers.OrderByDescending(u => u.Created) : orderBy(appUsers);
            
        
        if (filterBy != null) appUsers = appUsers.Where(filterBy);
        var count = await appUsers.CountAsync();
        if (page > 0 && pageSize > 0)
            appUsers = appUsers.Skip((page - 1) * pageSize).Take(pageSize);
        
        var pagination = Pagination.Generate(page, pageSize, count);
        var data = Mapper.Map<List<TUserDto>>(appUsers);
        foreach (var userDto in data)
        {
            var userClaims = await GetUserClaims(userDto.Id);
            userDto.Claims = Mapper.Map<List<TClaimDto>>(userClaims);
            userDto.Roles = await GetUserRolesIds(userDto.Id);
        }

        return new PaginatedResult<List<TUserDto>>
        {
            Pagination = pagination,
            Data = data
        };
    }

    /// <summary>
    /// Creates new user
    /// </summary>
    /// <param name="createDto"> User create dto </param>
    /// <returns> New user dto </returns>
    /// <exception cref="KirelIdentityStoreException"> If user or role managers fails on store based operations </exception>
    /// <exception cref="KirelValidationException"> If password validation failed </exception>
    public virtual async Task<TUserDto> CreateUser(TUserCreateDto createDto)
    {
        var appUser = Mapper.Map<TUser>(createDto);
        var result = await UserManager.CreateAsync(appUser);
        if (!result.Succeeded)
            throw new KirelIdentityStoreException("Failed to create new user");
        foreach (var roleId in createDto.Roles)
        {
            var role = await RoleManager.FindByIdAsync(roleId.ToString());
            if (createDto.Roles.Contains(role.Id)) await AddUserToRole(appUser.Id, roleId);
        }

        var claims = Mapper.Map<List<Claim>>(createDto.Claims);
        foreach (var claim in claims) await AddClaimToUser(claim, appUser.Id);
        var passwordResult = await UserManager.AddPasswordAsync(appUser, createDto.Password);
        if (passwordResult.Succeeded)
        {
            var newUser = Mapper.Map<TUserDto>(appUser);
            newUser.Roles = await GetUserRolesIds(appUser.Id);
            newUser.Claims = Mapper.Map<List<TClaimDto>>(claims);
            return newUser;
        }

        await UserManager.DeleteAsync(appUser);
        throw new KirelValidationException("Failed to add password");
    }

    /// <summary>
    /// Updates new user
    /// </summary>
    /// <param name="updateDto"> Update dto </param>
    /// <param name="userId"> User id </param>
    /// <returns> Updated user dto </returns>
    /// <exception cref="KirelNotFoundException"> If user with given id was not found </exception>
    /// <exception cref="KirelIdentityStoreException"> If user manager fails to update user </exception>
    public virtual async Task<TUserDto> UpdateUser(TKey userId, TUserUpdateDto updateDto)
    {
        var user = await UserManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new KirelNotFoundException($"User with specified id {userId} was not found");
        var updatedUser = Mapper.Map(updateDto, user);
        var result = await UserManager.UpdateAsync(updatedUser);
        if (!result.Succeeded)
            throw new KirelIdentityStoreException($"Failed to update user with {userId} id");
        await SyncUserClaims(userId, updateDto.Claims);
        await SyncUserRoles(userId, updateDto.Roles);
        if (!string.IsNullOrEmpty(updateDto.Password))
        {
            await UserManager.RemovePasswordAsync(user);
            await UserManager.AddPasswordAsync(user, updateDto.Password);
        }

        var returnDto = Mapper.Map<TUserDto>(updatedUser);
        returnDto.Claims = Mapper.Map<List<TClaimDto>>(await GetUserClaims(userId));
        returnDto.Roles = await GetUserRolesIds(userId);
        return returnDto;
    }

    /// <summary>
    /// Deletes user by id
    /// </summary>
    /// <param name="id"> User id </param>
    /// <exception cref="KirelNotFoundException"> If user with given id was not found </exception>
    public virtual async Task DeleteUser(TKey id)
    {
        var user = await UserManager.FindByIdAsync(id.ToString());
        if (user == null)
            throw new KirelNotFoundException($"User with specified id {id} was not found");
        await UserManager.DeleteAsync(user);
    }

    /// <summary>
    /// Adds a user to the specified role
    /// </summary>
    /// <param name="userId"> User id </param>
    /// <param name="roleId"> Role id </param>
    /// <exception cref="KirelNotFoundException"> if role or user was not found </exception>
    public virtual async Task AddUserToRole(TKey userId, TKey roleId)
    {
        var role = await RoleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
            throw new KirelNotFoundException($"Role with given id {roleId} was not found");
        var user = await UserManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new KirelNotFoundException($"User with specified id {userId} was not found");
        await UserManager.AddToRoleAsync(user, role.Name);
    }

    /// <summary>
    /// Deletes a user from the specified role
    /// </summary>
    /// <param name="userId"> User id </param>
    /// <param name="roleId"> Role id </param>
    /// <exception cref="KirelNotFoundException"> If role or user was not found </exception>
    public virtual async Task DeleteUserFromRole(TKey userId, TKey roleId)
    {
        var role = await RoleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
            throw new KirelNotFoundException($"Role with given id {roleId} was not found");
        var user = await UserManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new KirelNotFoundException($"User with specified id {userId} was not found");
        await UserManager.RemoveFromRoleAsync(user, role.Name);
    }
}