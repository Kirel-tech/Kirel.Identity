using Kirel.Identity.Core.Models;
using Kirel.Identity.DTOs;
using Kirel.Identity.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Services;

/// <summary>
/// Provides methods for getting the user within the authentication procedure using different methods
/// </summary>
/// <typeparam name="TKey"> User entity key type. </typeparam>
/// <typeparam name="TUser"> User entity type. </typeparam>
/// <typeparam name="TRole"> Role entity type. </typeparam>
/// <typeparam name="TUserRole"> Role user entity type. </typeparam>
/// <typeparam name="TUserClaim"> User claim type</typeparam>
/// <typeparam name="TRoleClaim"> Role claim type</typeparam>
public class KirelAuthenticationService<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
    where TUserClaim : IdentityUserClaim<TKey>
    where TRoleClaim : IdentityRoleClaim<TKey>
{
    /// <summary>
    /// Identity user manager
    /// </summary>
    protected readonly UserManager<TUser> UserManager;

    /// <summary>
    /// Constructor for KirelAuthenticationService
    /// </summary>
    /// <param name="userManager"> </param>
    public KirelAuthenticationService(UserManager<TUser> userManager)
    {
        UserManager = userManager;
    }

    /// <summary>
    /// Provides the ability to get the identity user after checking the login and password
    /// </summary>
    /// <param name="dto"> User authentication dto </param>
    /// <returns> User class instance </returns>
    /// <exception cref="KirelAuthenticationException"> Returned if the user is not found or if the password is incorrect </exception>
    /// <exception cref="KirelIdentityStoreException"> If user or role managers fails on store based operations </exception>
    public async Task<TUser> LoginByPassword(KirelUserAuthenticationDto dto)
    {
        TUser? user;
        switch (dto.Type.ToLower())
        {
            case "username":
                user = await UserManager.FindByNameAsync(dto.Login);
                break;
            case "phone":
                user = UserManager.Users.FirstOrDefault(u => u.PhoneNumber == dto.Login);
                break;
            case "email":
                user = await UserManager.FindByEmailAsync(dto.Login);
                break;
            default:
                throw new KirelAuthenticationException("Passed authentication type does not supported");
        }
        if (user == null) throw new KirelAuthenticationException("User with passed login is not found");
        var result = await UserManager.CheckPasswordAsync(user, dto.Password);
        if (!result) throw new KirelAuthenticationException("Wrong password");
        return user;
    }
}