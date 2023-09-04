using System.Net.Mail;
using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Services;

/// <summary>
/// Service for email confirmation.
/// </summary>
/// <typeparam name="TKey"> User key type. </typeparam>
/// <typeparam name="TUser"> User type. </typeparam>
/// <typeparam name="TRole"> Role entity type. </typeparam>
/// <typeparam name="TUserRole"> User role entity type. </typeparam>
public class KirelEmailConfirmationService<TKey, TUser, TRole, TUserRole>
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
    /// Initializes a new instance of the <see cref="KirelEmailConfirmationService{TKey, TUser, TRole, TUserRole}" /> class.
    /// </summary>
    /// <param name="userManager"> The user manager. </param>
    /// <param name="mailSender"> The mail sender. </param>
    public KirelEmailConfirmationService(UserManager<TUser> userManager, IMailSender mailSender)
    {
        UserManager = userManager;
        MailSender = mailSender;
    }

    /// <summary>
    /// Sends a confirmation email to the user.
    /// </summary>
    /// <param name="user"> The user to send the email to. </param>
    /// <param name="token"> The email confirmation token. </param>
    public virtual async Task SendConfirmationMail(TUser user, string token)
    {
        var link = $"https://localhost:7055/registration/confirm?Email={user.Email}&token={token}";
        var message = new MailMessage
        {
            Subject = "Email Confirmation",
            Body =
                $"Please confirm your email by clicking <a href='{link}'>here</a>. Please do not reply to this message.",
            IsBodyHtml = true
        };

        await MailSender.SendAsync(user.Email, message);
    }

    /// <summary>
    /// Confirms the email for a user.
    /// </summary>
    /// <param name="email"> The email address. </param>
    /// <param name="token"> The email confirmation token. </param>
    public virtual async Task ConfirmMail(string email, string token)
    {
        token = token.Replace(' ', '+');
        var user = await UserManager.FindByEmailAsync(email);
        var result = await UserManager.ConfirmEmailAsync(user, token);
    }
}