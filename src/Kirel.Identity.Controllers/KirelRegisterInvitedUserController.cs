using Kirel.Identity.Core.Models;
using Kirel.Identity.Core.Services;
using Kirel.Identity.DTOs;
using Kirel.Identity.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Kirel.Identity.Controllers;

/// <summary>
/// Controller for registration invited users
/// </summary>
/// <typeparam name="TRegistrationService"> User registration service type </typeparam>
/// <typeparam name="TRegistrationDto"> User registration DTO type </typeparam>
/// <typeparam name="TKey"> The user key type </typeparam>
/// <typeparam name="TUser"> The user type </typeparam>
/// <typeparam name="TRole"> The role entity type </typeparam>
/// <typeparam name="TUserRole"> The user role entity type </typeparam>
/// <typeparam name="TUserClaim"> User claim type. </typeparam>
/// <typeparam name="TRoleClaim"> Role claim type. </typeparam>
/// <typeparam name="TRegisterInvitedUserDto"> The register invited user dto type</typeparam>
public class KirelRegisterInvitedUserController<TRegistrationService, TRegisterInvitedUserDto, TRegistrationDto, TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim> : Controller
    where TRegistrationService : KirelRegistrationService<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim, TRegisterInvitedUserDto, TRegistrationDto>
    where TRegisterInvitedUserDto : KirelRegisterInvitedUserDto
    where TRegistrationDto : KirelUserRegistrationDto
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
    where TRoleClaim : KirelIdentityRoleClaim<TKey>
    where TUserClaim : KirelIdentityUserClaim<TKey> 
{
    /// <summary>
    /// Authorized user service
    /// </summary>
    protected readonly TRegistrationService Service;

    /// <summary>
    /// Constructor for KirelRegisterInvitedUserController
    /// </summary>
    /// <param name="service"> User registration service </param>
    public KirelRegisterInvitedUserController(TRegistrationService service)
    {
        Service = service;
    }

    /// <summary>
    /// Invited user registration
    /// </summary>
    /// <param name="registrationDto">User registration dto</param>
    /// <param name="userId">Invited user id</param>
    [HttpPost]
    public virtual async Task<ActionResult> RegisterInvitedUser(TRegisterInvitedUserDto registrationDto, Guid userId)
    {
        await Service.RegisterInvitedUser(registrationDto, userId);
        return NoContent();
    }
}