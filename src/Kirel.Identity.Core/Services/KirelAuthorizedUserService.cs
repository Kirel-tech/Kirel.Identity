using System.Security.Claims;
using AutoMapper;
using Kirel.Identity.Core.Models;
using Kirel.Identity.DTOs;
using Kirel.Identity.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Kirel.Identity.Core.Services;

/// <summary>
/// Service for receiving and changing authorized user account information
/// </summary>
/// <typeparam name="TKey"> User key type </typeparam>
/// <typeparam name="TUser"> User type </typeparam>
/// <typeparam name="TAuthorizedUserDto"> Authorized user dto type </typeparam>
/// <typeparam name="TAuthorizedUserUpdateDto"> Authorized user update dto type </typeparam>
/// <typeparam name="TRole"> Role entity type. </typeparam>
/// <typeparam name="TUserRole"> User role entity type. </typeparam>
/// <typeparam name="TUserClaim"> User claim type. </typeparam>
/// <typeparam name="TRoleClaim"> Role claim type. </typeparam>
public class KirelAuthorizedUserService<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim, 
    TAuthorizedUserDto, TAuthorizedUserUpdateDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
    where TRoleClaim : KirelIdentityRoleClaim<TKey>
    where TUserClaim : KirelIdentityUserClaim<TKey>
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

    protected TUser? User { get; private set; }

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
        User = GetUser();
    }

    private void CheckUser(TUser? user, string login)
    {
        switch (user)
        {
            case null:
                throw new KirelAuthenticationException($"User with login {login} is not found");
            case { LockoutEnabled: true, LockoutEnd: not null } when DateTime.Now.ToUniversalTime() < user.LockoutEnd.Value.ToUniversalTime():
                throw new KirelAuthenticationException($"User with login {login} is locked");
        }
    }
    
    private TUser? GetUser()
    {
        var userName = HttpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
        if (string.IsNullOrEmpty(userName))
            return null;
        var user = UserManager.Users.FirstOrDefault(u => u.UserName == userName);
        CheckUser(user, userName);
        return user;
    }

    private string GetUserName()
    {
        if (User == null || User.UserName.IsNullOrEmpty())
            throw new KirelUnauthorizedException("User not authorized");
        return User.UserName;
    }

    /// <summary>
    /// Gets a authorized user dto
    /// </summary>
    /// <returns> User dto </returns>
    /// <exception cref="KirelNotFoundException"> If user not fount in users store </exception>
    public virtual async Task<TAuthorizedUserDto> GetDto()
    {
        if (User == null || User.UserName.IsNullOrEmpty())
            throw new KirelUnauthorizedException("User not authorized");
        return Mapper.Map<TAuthorizedUserDto>(User);
    }

    /// <summary>
    /// Gets authorized user
    /// </summary>
    /// <returns> </returns>
    /// <exception cref="KirelNotFoundException"> If user not fount in users store </exception>
    public virtual async Task<TUser> Get()
    {
        if (User == null || User.UserName.IsNullOrEmpty())
            throw new KirelUnauthorizedException("User not authorized");
        return User;
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
        if (User == null || User.UserName.IsNullOrEmpty())
            throw new KirelUnauthorizedException("User not authorized");
        var updatedUser = Mapper.Map(updateDto, User);
        var result = await UserManager.UpdateAsync(updatedUser);
        if (!result.Succeeded) throw new KirelIdentityStoreException("User manager failed to update user");
        User = updatedUser;
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
        if (User == null || User.UserName.IsNullOrEmpty())
            throw new KirelUnauthorizedException("User not authorized");
        await UserManager.ChangePasswordAsync(User, currentPassword, newPassword);
    }
}