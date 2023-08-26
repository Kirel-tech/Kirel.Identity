using System.Net;
using System.Net.Mail;
using AutoMapper;
using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Models;
using Kirel.Identity.DTOs;
using Kirel.Identity.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Services
{
    /// <summary>
    /// Provides methods for registering users
    /// </summary>
    /// <typeparam name="TKey"> User key type </typeparam>
    /// <typeparam name="TUser"> User type </typeparam>
    /// <typeparam name="TRegistrationDto"> User registration dto type </typeparam>
    public class KirelRegistrationService<TKey, TUser, TRegistrationDto> : IKirelIdentityRegistrationService
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
        /// <param name="registrationDto"> Registration data transfer object </param>
        /// <exception cref="KirelIdentityStoreException"> If user or role managers fail on store-based operations </exception>
        public virtual async Task Registration(TRegistrationDto registrationDto)
        {
            var appUser = Mapper.Map<TUser>(registrationDto);
            var result = await UserManager.CreateAsync(appUser);

            if (!result.Succeeded)
                throw new KirelIdentityStoreException("Failed to create new user");

            var passwordResult = await UserManager.AddPasswordAsync(appUser, registrationDto.Password);

            // Generate the email confirmation token and confirmation link
            var token = await UserManager.GenerateEmailConfirmationTokenAsync(appUser);

            var confirmationLink = GenerateConfirmationLink(appUser.Email, token);
            // Send the confirmation email
            await SendConfirmationEmailAsync(appUser.Email, confirmationLink);

            if (!passwordResult.Succeeded)
            {
                await UserManager.DeleteAsync(appUser);
                throw new KirelIdentityStoreException("Failed to add password");
            }
        }

        /// <summary>
        /// Generates an email confirmation link
        /// </summary>
        /// <param name="email"> User's email </param>
        /// <param name="token"> Email confirmation token </param>
        /// <returns> The confirmation link </returns>
        public string GenerateConfirmationLink(string email, string token)
        {
            return $"https://localhost:7055/registration/confirm?Email={email}&token={token}";
        }

        /// <summary>
        /// Sends a confirmation email
        /// </summary>
        /// <param name="recipientEmail"> Recipient's email address </param>
        /// <param name="confirmationLink"> Confirmation link </param>
        /// <returns> A task representing the asynchronous operation </returns>
        public async Task SendConfirmationEmailAsync(string recipientEmail, string confirmationLink)
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
                Body = $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>, please don't reply to this message.",
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
        /// Confirm user's email address
        /// </summary>
        /// <param name="token"> Email confirmation token </param>
        /// <param name="email"> User's email address </param>
        /// <exception cref="Exception"> If email confirmation fails </exception>
        public async Task EmailConfirm(string token, string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            token = token.Replace(' ', '+');
            var result = await UserManager.ConfirmEmailAsync(user, token);
        }
    }
}
