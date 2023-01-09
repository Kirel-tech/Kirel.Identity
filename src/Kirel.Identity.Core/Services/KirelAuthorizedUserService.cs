using AutoMapper;
using Kirel.Identity.DTOs;
using Kirel.Identity.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Services;

/// <summary>
/// Service for receiving and changing authorized user account information
/// </summary>
/// <typeparam name="TKey">User key type</typeparam>
/// <typeparam name="TUser">User type</typeparam>
/// <typeparam name="TAuthorizedUserDto">Authorized user dto type</typeparam>
/// <typeparam name="TAuthorizedUserUpdateDto">Authorized user update dto type</typeparam>
public class KirelAuthorizedUserService<TKey, TUser, TAuthorizedUserDto, TAuthorizedUserUpdateDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : IdentityUser<TKey>, IKirelUser<TKey>
    where TAuthorizedUserDto : KirelAuthorizedUserDto
    where TAuthorizedUserUpdateDto : KirelAuthorizedUserUpdateDto
{
    /// <summary>
    /// Identity user manager
    /// </summary>
    protected readonly UserManager<TUser> UserManager;
    /// <summary>
    /// AutoMapper instance
    /// </summary>
    protected readonly IMapper Mapper;
    
    /// <summary>
    /// KirelAuthorizedUserService constructor
    /// </summary>
    /// <param name="userManager">Identity user manager</param>
    /// <param name="mapper">AutoMapper instance</param>
    public KirelAuthorizedUserService(UserManager<TUser> userManager, IMapper mapper)
    {
        UserManager = userManager;
        Mapper = mapper;
    }
    
    /// <summary>
    /// Gets a user by id
    /// </summary>
    /// <param name="userId">User id</param>
    /// <returns>User dto</returns>
    /// <exception cref="KeyNotFoundException">If user with given id was not found</exception>
    public virtual async Task<TAuthorizedUserDto> GetById(TKey userId)
    {
        var user = await UserManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new KeyNotFoundException($"User with specified id {userId} was not found");
        return Mapper.Map<TAuthorizedUserDto>(user);
    }
    
    /// <summary>
    /// Updates new user
    /// </summary>
    /// <param name="updateDto">Update dto</param>
    /// <param name="userId">User id</param>
    /// <returns>Updated user dto</returns>
    /// <exception cref="KeyNotFoundException">If user with given id was not found</exception>
    public virtual async Task<TAuthorizedUserDto> UpdateUser(TKey userId, TAuthorizedUserUpdateDto updateDto)
    {
        var user = await UserManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new KeyNotFoundException($"User with specified id {userId} was not found");
        var updatedUser = Mapper.Map(updateDto, user);
        var result = await UserManager.UpdateAsync(updatedUser);
        if (!result.Succeeded) throw new Exception("User manager failed to update user");
        return Mapper.Map<TAuthorizedUserDto>(updatedUser);
    }
    
    /// <summary>
    /// Changes user password
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="currentPassword">Current user password</param>
    /// <param name="newPassword">New user password</param>
    /// <exception cref="KeyNotFoundException">If user with given id was not found</exception>
    public virtual async Task ChangeUserPassword(TKey userId, string currentPassword, string newPassword)
    {
        var user = await UserManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new KeyNotFoundException($"User with specified id {userId} was not found");
        await UserManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }
}