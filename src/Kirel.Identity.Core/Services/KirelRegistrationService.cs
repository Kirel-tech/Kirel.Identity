using System.Net;
using System.Net.Mail;
using AutoMapper;
using Kirel.Identity.Core.Models;
using Kirel.Identity.DTOs;
using Kirel.Identity.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Services;

/// <summary>
/// Provides methods for registering users
/// </summary>
/// <typeparam name="TKey"> User key type </typeparam>
/// <typeparam name="TUser"> User type </typeparam>
/// <typeparam name="TRegistrationDto"> User registration dto type </typeparam>
public class KirelRegistrationService<TKey, TUser, TRegistrationDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey>
    where TRegistrationDto : KirelUserRegistrationDto
{
    /// <summary>
    /// AutoMapper instance
    /// </summary>
    protected readonly IMapper Mapper;

    /// <summary>
    /// Identity user manager
    /// </summary> 
    protected readonly UserManager<TUser> UserManager;

    /// <summary>
    /// Constructor for KirelRegistrationService
    /// </summary>
    /// <param name="userManager"> Identity user manager </param>
    /// <param name="mapper"> AutoMapper instance </param>
    public KirelRegistrationService(UserManager<TUser> userManager, IMapper mapper)
    {
        UserManager = userManager;
        Mapper = mapper;
    }

    /// <summary>
    /// User registration method
    /// </summary>
    /// <param name="registrationDto"> registration data transfer object </param>
    /// <exception cref="KirelIdentityStoreException"> If user or role managers fails on store based operations </exception>
    public virtual async Task Registration(TRegistrationDto registrationDto)
    {
        var appUser = Mapper.Map<TUser>(registrationDto);
        var result = await UserManager.CreateAsync(appUser);
            
        if (!result.Succeeded)
            throw new KirelIdentityStoreException("Failed to create new user");

        // Generate the email confirmation token and confirmation link
        var token = await UserManager.GenerateUserTokenAsync(appUser, "Default", "EmailConfirmation");
        var confirmationLink = GenerateConfirmationLink(appUser.Id, token);
        // Send the confirmation email
        await SendConfirmationEmailAsync(appUser.Email, confirmationLink);

        var passwordResult = await UserManager.AddPasswordAsync(appUser, registrationDto.Password);

        if (!passwordResult.Succeeded)
        {
            await UserManager.DeleteAsync(appUser);
            throw new KirelIdentityStoreException("Failed to add password");
        }
    }
    
    private string GenerateConfirmationLink(TKey userId, string token)
    {
        return $"https://localhost:7055/registration/confirm?userId={userId}&token={token}";
    }
    private async Task SendConfirmationEmailAsync(string recipientEmail, string confirmationLink)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("dont.even.try.to.replybltch@gmail.com", "zetyhgdcvboswjbf"),
            EnableSsl = true,
            UseDefaultCredentials = false
        };

        var message = new MailMessage
        {
            From = new MailAddress("dont.even.try.to.replybltch@gmail.com"),
            Subject = "Email Confirmation",
            Body = $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>, please dont reply this message .",
            IsBodyHtml = true,
        };

        message.To.Add(recipientEmail);

        try
        {
            await smtpClient.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
        finally
        {
            smtpClient.Dispose();
            message.Dispose();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="token"></param>
    /// <exception cref="Exception"></exception>
    public async Task ConfirmEmailAsync(TKey userId, string token)
    {
        // Проверьте наличие userId и token, а затем вызовите ConfirmEmailAsync
        var user = await UserManager.FindByIdAsync(userId.ToString());
        if (user != null)
        {
            var result = await UserManager.VerifyUserTokenAsync(user,"Default","EmailConfirmation", token);
            if (!result)
            {   
                // Обработка ошибки подтверждения email
                throw new Exception("Email confirmation failed.");
            }
        }
        else
        {
            // Обработка ошибки: пользователь не найден
            throw new Exception("User not found.");
        }
    }
    
}

