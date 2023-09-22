using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Models;
using Kirel.Identity.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Services;

/// <summary>
/// Service for sms authentication
/// </summary>
public class KirelSmsAuthenticationService<TKey, TUser, TRole, TUserRole, TUserClaim,TRoleClaim>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
    where TUserClaim : IdentityUserClaim<TKey>
    where TRoleClaim : IdentityRoleClaim<TKey>
{
    /// <summary>
    /// ISmsSender implementation
    /// </summary>
    protected readonly IKirelSmsSender KirelSmsSender;
    /// <summary>
    /// Identity user manager
    /// </summary>
    protected readonly UserManager<TUser> UserManager;

    /// <summary>
    /// Constructor for KirelSmsAuthenticationService
    /// </summary>
    /// <param name="userManager">Identity user manager</param>
    /// <param name="kirelSmsSender">ISmsSender implementation</param>
    public KirelSmsAuthenticationService(UserManager<TUser> userManager, IKirelSmsSender kirelSmsSender)
    {
        UserManager = userManager;
        KirelSmsSender = kirelSmsSender;
    }
    
    /// <summary>
    /// Login by phone number and single-use 4-digit code
    /// </summary>
    /// <param name="phoneNumber">User phone number</param>
    /// <param name="code">Single-use 4-digit code</param>
    /// <returns></returns>
    public async Task<TUser> LoginByCode(string phoneNumber, string code)
    {
        var user = UserManager.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
        if (user == null)
            throw new KirelNotFoundException("User with given phone number was not found"); //!user.IsRegistrationFinished
        var numberConfirmed = await UserManager.IsPhoneNumberConfirmedAsync(user);
        if (!numberConfirmed) 
            throw new KirelUnauthorizedException("Your phone number is not verified");
        if (!user.IsRegistrationFinished) 
            throw new KirelUnauthorizedException("Finish your registration before log in");
        var result = await UserManager.VerifyUserTokenAsync(user, "Code provider", "PhoneAuthentication", code);
        if (!result) throw new KirelUnauthorizedException("Invalid token");
        return user;
    }

    /// <summary>
    /// Sends single-use 4-digit code to the given phone number
    /// </summary>
    /// <param name="phoneNumber">User phone number</param>
    public async Task SendCodeBySms(string phoneNumber)
    {
        var user = UserManager.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
        if (user == null)
            throw new KirelNotFoundException("User with given phone number was not found");
        
        if (!user.PhoneNumberConfirmed)
            throw new Exception("Your phone number is not verified");
        var code = await UserManager.GenerateUserTokenAsync(user, "Code provider", "PhoneAuthentication");
        var text = $"Please enter the following code on the login page: {code}";

        await KirelSmsSender.SendSms(text, phoneNumber);
    }
}