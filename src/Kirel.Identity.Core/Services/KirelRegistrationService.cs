﻿using System.Security.Authentication;
using AutoMapper;
using Kirel.Identity.Core.Models;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Services;

/// <summary>
/// Provides methods for registering users
/// </summary>
/// <typeparam name="TKey">User key type</typeparam>
/// <typeparam name="TUser">User type</typeparam>
/// <typeparam name="TRegistrationDto">User registration dto type</typeparam>
public class KirelRegistrationService<TKey, TUser, TRegistrationDto> 
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey> 
    where TUser : KirelIdentityUser<TKey>
    where TRegistrationDto : KirelUserRegistrationDto
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
    /// Constructor for KirelRegistrationService
    /// </summary>
    /// <param name="userManager">Identity user manager</param>
    /// <param name="mapper">AutoMapper instance</param>
    public KirelRegistrationService(UserManager<TUser> userManager, IMapper mapper)
    {
        UserManager = userManager;
        Mapper = mapper;
    }
    
    /// <summary>
    /// User registration method
    /// </summary>
    /// <param name="registrationDto">registration data transfer object</param>
    /// <exception cref="AuthenticationException">if user creation failed</exception>
    public virtual async Task Registration(TRegistrationDto registrationDto)
    {
        var appUser = Mapper.Map<TUser>(registrationDto);
        var result = await UserManager.CreateAsync(appUser);
        //TODO: change exception types
        if(!result.Succeeded)  throw new AuthenticationException("Failed to create new user");
        var passwordResult = await UserManager.AddPasswordAsync(appUser, registrationDto.Password);
        if (!passwordResult.Succeeded)
        {
            await UserManager.DeleteAsync(appUser);
            //TODO: change exception types
            throw new AuthenticationException("Failed to add password");
        }
    }
}