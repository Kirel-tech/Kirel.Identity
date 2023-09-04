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
/// <typeparam name="TEmailConfirmationService"> </typeparam>
public class KirelRegistrationController<TRegistrationService, TRegistrationDto, TKey, TUser, TRole, TUserRole,
    TEmailConfirmationService> : Controller
    where TRegistrationService : KirelRegistrationService<TKey, TUser, TRole, TUserRole, TRegistrationDto>
    where TRegistrationDto : KirelUserRegistrationDto
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole>
    where TEmailConfirmationService : KirelEmailConfirmationService<TKey, TUser, TRole, TUserRole>
{
    /// <summary>
    /// The service responsible for email confirmation operations.
    /// </summary>
    protected readonly TEmailConfirmationService _emailConfirmationService;

    /// <summary>
    /// Authorized user service
    /// </summary>
    protected readonly TRegistrationService Service;

    /// <summary>
    /// Constructor for KirelRegistrationController
    /// </summary>
    /// <param name="service"> User registration service </param>
    /// <param name="emailConfirmationService">emailConfirmation service </param>
    public KirelRegistrationController(TRegistrationService service, TEmailConfirmationService emailConfirmationService)
    {
        Service = service;
        _emailConfirmationService = emailConfirmationService;
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

    /// <summary>
    /// Confirms the user's email based on the provided token.
    /// </summary>
    /// <param name="Email">The email address of the user.</param>
    /// <param name="token">The email confirmation token.</param>
    /// <returns>An IActionResult indicating the result of the email confirmation.</returns>
    [HttpGet("confirm")]
    public async Task<IActionResult> ConfirmEmail(string Email, string token)
    {
        try
        {
            await _emailConfirmationService.ConfirmMail(Email, token);
        }
        catch (Exception ex)
        {
            // Handle email confirmation failure and return a BadRequest result with an error message.
            return BadRequest($"Email confirmation failed: {ex.Message}");
        }
        return Ok("Email confirmed successfully.");
    }

}