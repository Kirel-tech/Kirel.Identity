using System.Security.Claims;
using AutoMapper;
using Kirel.Identity.Core.Models;
using Kirel.Identity.DTOs;
using Kirel.Identity.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Services;

/// <summary>
/// Service for receiving and changing authorized user account information
/// </summary>
/// <typeparam name="TKey"> User key type </typeparam>
/// <typeparam name="TUser"> User type </typeparam>
/// <typeparam name="TAuthorizedUserDto"> Authorized user dto type </typeparam>
/// <typeparam name="TAuthorizedUserUpdateDto"> Authorized user update dto type </typeparam>
public class KirelAuthorizedUserService<TKey, TUser, TAuthorizedUserDto, TAuthorizedUserUpdateDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey>
    where TAuthorizedUserDto : KirelAuthorizedUserDto
    where TAuthorizedUserUpdateDto : KirelAuthorizedUserUpdateDto
{
    /// <summary>
    /// HttpContextAccessor
    /// </summary>
    protected readonly IHttpContextAccessor HttpContextAccessor;

    /// <summary>
    /// AutoMapper instance
    /// </summary>
    protected readonly IMapper Mapper;

    /// <summary>
    /// Identity user manager
    /// </summary>
    protected readonly UserManager<TUser> UserManager;

    /// <summary>
    /// KirelAuthorizedUserService constructor
    /// </summary>
    /// <param name="httpContextAccessor"> Http context accessor </param>
    /// <param name="userManager"> Identity user manager </param>
    /// <param name="mapper"> AutoMapper instance </param>
    public KirelAuthorizedUserService(IHttpContextAccessor httpContextAccessor, UserManager<TUser> userManager,
        IMapper mapper)
    {
        UserManager = userManager;
        Mapper = mapper;
        HttpContextAccessor = httpContextAccessor;
    }

    private string GetUserName()
    {
        var userName = HttpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
        if (string.IsNullOrEmpty(userName))
            throw new KirelUnauthorizedException("User not authorized");
        return userName;
    }

    /// <summary>
    /// Gets a authorized user dto
    /// </summary>
    /// <returns> User dto </returns>
    /// <exception cref="KirelNotFoundException"> If user not fount in users store </exception>
    public virtual async Task<TAuthorizedUserDto> GetDto()
    {
        var userName = GetUserName();
        var user = await UserManager.FindByNameAsync(userName);
        if (user == null)
            throw new KirelNotFoundException($"User with username {userName} was not found");
        return Mapper.Map<TAuthorizedUserDto>(user);
    }

    /// <summary>
    /// Gets authorized user
    /// </summary>
    /// <returns> </returns>
    /// <exception cref="KirelNotFoundException"> If user not fount in users store </exception>
    public virtual async Task<TUser> Get()
    {
        var userName = GetUserName();
        var user = await UserManager.FindByNameAsync(userName);
        if (user == null)
            throw new KirelNotFoundException($"User with username {userName} was not found");
        return user;
    }

    /// <summary>
    /// Updates authorized user
    /// </summary>
    /// <param name="updateDto"> Update dto </param>
    /// <returns> Updated user dto </returns>
    /// <exception cref="KirelNotFoundException"> If user with given id was not found </exception>
    /// <exception cref="KirelIdentityStoreException"> If user or role managers fails on store based operations </exception>
    public virtual async Task<TAuthorizedUserDto> Update(TAuthorizedUserUpdateDto updateDto)
    {
        var userName = GetUserName();
        var user = await UserManager.FindByNameAsync(userName);
        if (user == null)
            throw new KirelNotFoundException($"User with username {userName} was not found");
        var updatedUser = Mapper.Map(updateDto, user);
        var result = await UserManager.UpdateAsync(updatedUser);
        if (!result.Succeeded) throw new KirelIdentityStoreException("User manager failed to update user");
        return Mapper.Map<TAuthorizedUserDto>(updatedUser);
    }

    /// <summary>
    /// Changes user password
    /// </summary>
    /// <param name="currentPassword"> Current user password </param>
    /// <param name="newPassword"> New user password </param>
    /// <exception cref="KirelNotFoundException"> If user with given id was not found </exception>
    public virtual async Task ChangeUserPassword(string currentPassword, string newPassword)
    {
        var username = GetUserName();
        var user = await UserManager.FindByNameAsync(username);
        if (user == null)
            throw new KirelNotFoundException($"User with username {username} was not found");
        await UserManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }
}