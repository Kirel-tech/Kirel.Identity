using System.Net.Mail;
using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Models;
using Kirel.Identity.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Services;

/// <summary>
/// Service for authentication via email.
/// </summary>
/// <typeparam name="TKey"> The type of the user key. </typeparam>
/// <typeparam name="TUser"> The type of the user. </typeparam>
/// <typeparam name="TRole"> The type of the role. </typeparam>
/// <typeparam name="TUserRole"> The type of the user role. </typeparam>
public class KirelEmailAuthenticationService<TKey, TUser, TRole, TUserRole>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole>
{
    /// <summary>
    /// Mail sender.
    /// </summary>
    protected readonly IMailSender MailSender;

    /// <summary>
    /// Identity user manager.
    /// </summary>
    protected readonly UserManager<TUser> UserManager;

    /// <summary>
    /// User token storage.
    /// </summary>
    private protected KirelUserToken<TKey> KirelUserToken;

    /// <summary>
    /// Initializes a new instance of the <see cref="KirelEmailAuthenticationService{TKey,TUser,TRole,TUserRole}" />
    /// class.
    /// </summary>
    /// <param name="userManager"> The user manager. </param>
    /// <param name="mailSender"> The mail sender. </param>
    /// <param name="kirelUserToken"> The user token storage. </param>
    public KirelEmailAuthenticationService(UserManager<TUser> userManager, IMailSender mailSender,
        KirelUserToken<TKey> kirelUserToken)
    {
        UserManager = userManager;
        MailSender = mailSender;
        KirelUserToken = kirelUserToken;
    }

    /// <summary>
    /// Generates a token for a user.
    /// </summary>
    /// <param name="user"> The user for whom to generate the token. </param>
    /// <returns> The generated token. </returns>
    public virtual async Task<string> GenerateToken(TUser user)
    {
        if (!user.EmailConfirmed)
        {
            throw new Exception("Your email address is not verified");
        }

        var token = KirelUserToken.GenerateToken(user.Id, "Email");
        return token;
    }


    /// <summary>
    /// Validates a token for a user.
    /// </summary>
    /// <param name="email"> The user to validate the token for. </param>
    /// <param name="token"> The token to validate. </param>
    /// <returns> True if the token is valid; otherwise, false. </returns>
    public virtual async Task<TUser> LoginByToken(string email, string token)
    {
        var user = await UserManager.FindByEmailAsync(email);
        var res = KirelUserToken.ValidateToken(user.Id, "Email", token);
        // If the confirmation fails, throw an exception
        if (!res) throw new KirelUnauthorizedException("Invalid token");
        return user;
    }

    /// <summary>
    /// Sends a confirmation email to the user.
    /// </summary>
    /// <param name="email"> The user to send the email to. </param>
    public virtual async Task SendTokenOnMail(string email)
    {
        var user = await UserManager.FindByEmailAsync(email);
        var token = await GenerateToken(user);

        var message = new MailMessage
        {
            Subject = "Email Confirmation",
            Body = $"Please enter your code on the website to login: {token}. Please do not reply to this message.",
            IsBodyHtml = true
        };

        await MailSender.SendAsync(user.Email, message);
    }
}