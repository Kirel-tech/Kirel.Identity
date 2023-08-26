namespace Kirel.Identity.Core.Interfaces;
using System.Threading.Tasks;

/// <summary>
/// Provides methods for user registration.
/// </summary>
public interface IKirelIdentityRegistrationService
{
    /// <summary>
    /// Generates a confirmation email link.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="token">The email confirmation token.</param>
    /// <returns>The email confirmation link.</returns>
    string GenerateConfirmationLink(string email, string token);

    /// <summary>
    /// Sends an email with a confirmation request.
    /// </summary>
    /// <param name="recipientEmail">The recipient's email address.</param>
    /// <param name="confirmationLink">The confirmation email link.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendConfirmationEmailAsync(string recipientEmail, string confirmationLink);

    /// <summary>
    /// Confirms the user's email address.
    /// </summary>
    /// <param name="token">The email confirmation token.</param>
    /// <param name="email">The user's email address.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task EmailConfirm(string token, string email);
}
