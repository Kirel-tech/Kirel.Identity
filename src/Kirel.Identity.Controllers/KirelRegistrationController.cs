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
/// <typeparam name="TKey"> User key type </typeparam>
/// <typeparam name="TUser"> User type </typeparam>
public class KirelRegistrationController<TRegistrationService, TRegistrationDto, TKey, TUser> : Controller
    where TRegistrationService : KirelRegistrationService<TKey, TUser, TRegistrationDto>
    where TRegistrationDto : KirelUserRegistrationDto
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey>
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