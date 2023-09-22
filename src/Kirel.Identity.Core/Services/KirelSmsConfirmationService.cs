using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Models;
using Kirel.Identity.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Services;

/// <summary>
/// Service for user phone confirmation
/// </summary>
/// <typeparam name="TKey"> User key type </typeparam>
/// <typeparam name="TUser"> User type </typeparam>
/// <typeparam name="TRole"> The role entity type </typeparam>
/// <typeparam name="TUserRole"> The user role entity type </typeparam>
/// <typeparam name="TUserClaim"> User claim type</typeparam>
/// <typeparam name="TRoleClaim"> Role claim type</typeparam>
public class KirelSmsConfirmationService<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
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
    /// Implementation of the ISmsSender interface
    /// </summary>
    protected readonly IKirelSmsSender KirelSmsSender;

    /// <summary>
    /// Constructor for KirelSmsConfirmationService
    /// </summary>
    /// <param name="userManager">Identity user manager</param>
    /// <param name="kirelSmsSender">Implementation of the ISmsSender interface</param>
    public KirelSmsConfirmationService(UserManager<TUser> userManager, IKirelSmsSender kirelSmsSender)
    {
        UserManager = userManager;
        KirelSmsSender = kirelSmsSender;
    }

    /// <summary>
    /// Sends confirmation code to the user non confirmed phone number
    /// </summary>
    /// <param name="phoneNumber">User phone number</param>
    /// <exception cref="KirelUnauthorizedException">If user phone number already confirmed</exception>
    public async Task SendConfirmationSms(string phoneNumber)
    {
        var user = UserManager.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
        if (user == null) throw new KirelNotFoundException("User with passed phone number was not found");
        if (user.PhoneNumberConfirmed)
            throw new KirelUnauthorizedException("Your phone number has already been confirmed");
        var token = await UserManager.GenerateUserTokenAsync(user, "Code provider", "PhoneConfirmation");
        var text = $"Please enter the following code on the login page: {token}";

        await KirelSmsSender.SendSms(text, user.PhoneNumber);
    }

    /// <summary>
    /// Confirms user phone number by validating given code
    /// </summary>
    /// <param name="phoneNumber">User phone number</param>
    /// <param name="code">Confirmation code</param>
    /// <exception cref="KirelUnauthorizedException">If given code is invalid</exception>
    /// <exception cref="KirelIdentityStoreException">If identity store failed to update user</exception>
    public async Task ConfirmPhoneNumber(string phoneNumber, string code)
    {
        var user = UserManager.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
        if (user == null) throw new KirelNotFoundException("User with passed phone number was not found");
        var codeValid = await UserManager.VerifyUserTokenAsync(user, "Code provider", "PhoneConfirmation", code);
        if (!codeValid) 
            throw new KirelUnauthorizedException("Invalid code");

        user.PhoneNumberConfirmed = true;
        var result = await UserManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new KirelIdentityStoreException("Failed to update user field 'PhoneNumberConfirmed'");
    }
}