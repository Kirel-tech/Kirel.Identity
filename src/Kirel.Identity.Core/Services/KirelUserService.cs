using System.Linq.Expressions;
using AutoMapper;
using Kirel.DTO;
using Kirel.Identity.Core.Models;
using Kirel.Identity.DTOs;
using Kirel.Identity.Exceptions;
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
/// <typeparam name="TUserRole"> User role entity type. </typeparam>
/// <typeparam name="TUserClaim"> User claim type. </typeparam>
/// <typeparam name="TRoleClaim"> Role claim type. </typeparam>
public class KirelUserService<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim, TUserDto, TUserCreateDto, TUserUpdateDto, TClaimDto, TClaimCreateDto,
    TClaimUpdateDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
    where TRoleClaim : KirelIdentityRoleClaim<TKey>
    where TUserClaim : KirelIdentityUserClaim<TKey>
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
    /// Identity user manager
    /// </summary>
    protected readonly UserManager<TUser> UserManager;
    /// <summary>
    /// Users queryable instance
    /// </summary>
    protected virtual IQueryable<TUser> Users
    {
        get => UserManager.Users;
    }

    /// <summary>
    /// KirelUserService constructor
    /// </summary>
    /// <param name="userManager"> Identity user manager </param>
    /// <param name="mapper"> AutoMapper instance </param>
    public KirelUserService(UserManager<TUser> userManager, IMapper mapper)
    {
        UserManager = userManager;
        Mapper = mapper;
    }

    /// <summary>
    /// Gets a user by id
    /// </summary>
    /// <param name="userId"> User id </param>
    /// <returns> User dto </returns>
    /// <exception cref="KirelNotFoundException"> If user with given id was not found </exception>
    public virtual async Task<TUserDto> GetById(TKey userId)
    {
        var user = await Users.FirstOrDefaultAsync(u => u.Id.Equals(userId));
        if (user == null)
            throw new KirelNotFoundException($"User with specified id {userId} was not found");
        var userDto = Mapper.Map<TUserDto>(user);
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
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;
        Expression<Func<TUser, bool>>? searchExpression = _ => true;
        Expression<Func<TUser, bool>>? rolesIdsExpression = _ => true;
        Func<IQueryable<TUser>, IOrderedQueryable<TUser>>? orderByFunc = null;
        if (roleIds != null && roleIds.Any())
            rolesIdsExpression = u => u.UserRoles.Any(d => roleIds.Contains(d.RoleId));
        
        if (!string.IsNullOrEmpty(search))
            searchExpression = PredicateBuilder.PredicateSearchInAllFields<TUser>(search, false);
        
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
        var appUsers = Users;
        appUsers = orderBy == null ? appUsers.OrderByDescending(u => u.Created) : orderBy(appUsers);
        
        if (filterBy != null) appUsers = appUsers.Where(filterBy);
        var count = await appUsers.CountAsync();
        
        appUsers = appUsers.Skip((page - 1) * pageSize).Take(pageSize);
        
        var pagination = Pagination.Generate(page, pageSize, count);
        var data = Mapper.Map<List<TUserDto>>(appUsers);
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
        await UserManager.CreateAsync(appUser);
        var result = await UserManager.AddPasswordAsync(appUser, createDto.Password);
        if (!result.Succeeded)
        {
            await UserManager.DeleteAsync(appUser);
            throw new KirelValidationException("Failed to add password");
        }
        return Mapper.Map<TUserDto>(appUser);
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
        var user = await Users.FirstOrDefaultAsync(u => u.Id.Equals(userId));
        if (user == null)
            throw new KirelNotFoundException($"User with specified id {userId} was not found");
        var updatedUser = Mapper.Map(updateDto, user);
        var result = await UserManager.UpdateAsync(updatedUser);
        if (!result.Succeeded)
            throw new KirelIdentityStoreException($"Failed to update user with {userId} id");
        if (!string.IsNullOrEmpty(updateDto.Password))
        {
            await UserManager.RemovePasswordAsync(user);
            await UserManager.AddPasswordAsync(user, updateDto.Password);
        }
        var returnDto = Mapper.Map<TUserDto>(updatedUser);
        return returnDto;
    }

    /// <summary>
    /// Deletes user by id
    /// </summary>
    /// <param name="id"> User id </param>
    /// <exception cref="KirelNotFoundException"> If user with given id was not found </exception>
    public virtual async Task DeleteUser(TKey id)
    {
        var user = await Users.FirstOrDefaultAsync(u => u.Id.Equals(id));
        if (user == null)
            throw new KirelNotFoundException($"User with specified id {id} was not found");
        await UserManager.DeleteAsync(user);
    }
}