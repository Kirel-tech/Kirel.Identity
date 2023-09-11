using AutoMapper;
using Kirel.Identity.Core.Models;
using Kirel.Identity.DTOs;
using Kirel.Identity.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Services;

/// <summary>
/// Provides methods for registering users
/// </summary>
/// <typeparam name="TKey"> User key type </typeparam>
/// <typeparam name="TUser"> User type </typeparam>
/// <typeparam name="TRegistrationDto"> User registration dto type </typeparam>
/// <typeparam name="TRole"> Role entity type. </typeparam>
/// <typeparam name="TUserRole"> User role entity type. </typeparam>
/// <typeparam name="TUserClaim"> User claim type. </typeparam>
/// <typeparam name="TRoleClaim"> Role claim type. </typeparam>
public class KirelRegistrationService<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim, TRegistrationDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
    where TRoleClaim : KirelIdentityRoleClaim<TKey>
    where TUserClaim : KirelIdentityUserClaim<TKey>
    where TRegistrationDto : KirelUserRegistrationDto
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
    /// Constructor for KirelRegistrationService
    /// </summary>
    /// <param name="userManager"> Identity user manager </param>
    /// <param name="mapper"> AutoMapper instance </param>
    public KirelRegistrationService(UserManager<TUser> userManager, IMapper mapper)
    {
        UserManager = userManager;
        Mapper = mapper;
    }

    /// <summary>
    /// User registration method
    /// </summary>
    /// <param name="registrationDto"> registration data transfer object </param>
    /// <exception cref="KirelIdentityStoreException"> If user or role managers fails on store based operations </exception>
    public virtual async Task Registration(TRegistrationDto registrationDto)
    {
        var appUser = Mapper.Map<TUser>(registrationDto);
        var result = await UserManager.CreateAsync(appUser);
        if (!result.Succeeded) throw new KirelIdentityStoreException("Failed to create new user");
        var passwordResult = await UserManager.AddPasswordAsync(appUser, registrationDto.Password);
        if (!passwordResult.Succeeded)
        {
            await UserManager.DeleteAsync(appUser);
            throw new KirelIdentityStoreException("Failed to add password");
        }
    }
}