using Kirel.Identity.Core.Models;
using Kirel.Identity.Core.Services;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Kirel.Identity.Controllers;

/// <summary>
/// Controller for user registration
/// </summary>
/// <typeparam name="TRegistrationService"> User registration service type </typeparam>
/// <typeparam name="TRegistrationDto"> User registration DTO type </typeparam>
/// <typeparam name="TKey"> The user key type </typeparam>
/// <typeparam name="TUser"> The user type </typeparam>
/// <typeparam name="TRole"> The role entity type </typeparam>
/// <typeparam name="TUserRole"> The user role entity type </typeparam>
/// <typeparam name="TUserClaim"> User claim type. </typeparam>
/// <typeparam name="TRoleClaim"> Role claim type. </typeparam>
public class KirelRegistrationController<TRegistrationService, TRegistrationDto, TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim> : Controller
    where TRegistrationService : KirelRegistrationService<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim, TRegistrationDto>
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
    /// Constructor for KirelRegistrationController
    /// </summary>
    /// <param name="service"> User registration service </param>
    public KirelRegistrationController(TRegistrationService service)
    {
        Service = service;
    }

    /// <summary>
    /// User registration
    /// </summary>
    /// <param name="registrationDto"> </param>
    /// <returns> </returns>
    [HttpPost]
    public virtual async Task<ActionResult> Registration(TRegistrationDto registrationDto)
    {
        await Service.Registration(registrationDto);
        return NoContent();
    }
}