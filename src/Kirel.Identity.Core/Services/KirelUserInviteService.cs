using AutoMapper;
using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Models;
using Kirel.Identity.DTOs;
using Kirel.Identity.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Kirel.Identity.Core.Services;

/// <summary>
/// Service for invite users by email or sms
/// </summary>
/// <typeparam name="TKey"> Id type </typeparam>
/// <typeparam name="TUser"> User type. Must be an implementation of the KirelIdentityUser class </typeparam>
/// <typeparam name="TRole"> Role type. Must be an implementation of the KirelIdentityRole class </typeparam>
/// <typeparam name="TUserRole"> User role entity type. </typeparam>
/// <typeparam name="TUserClaim"> User claim type. </typeparam>
/// <typeparam name="TRoleClaim"> Role claim type. </typeparam>
/// <typeparam name="TUserInviteDto">User invite dto type</typeparam>
public class KirelUserInviteService<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim, TUserInviteDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
    where TUserClaim : IdentityUserClaim<TKey>
    where TRoleClaim : IdentityRoleClaim<TKey>
    where TUserInviteDto : KirelUserInviteDto
{
    /// <summary>
    /// Identity user manager
    /// </summary>
    protected readonly UserManager<TUser> UserManager;
    /// <summary>
    /// AutoMapper instance
    /// </summary>
    protected readonly IMapper Mapper;
    /// <summary>
    /// IKirelEmailSender instance
    /// </summary>
    protected readonly IKirelEmailSender EmailSender;    
    /// <summary>
    /// IKirelSmsSender instance
    /// </summary>
    protected readonly IKirelSmsSender SmsSender;
    /// <summary>
    /// Options for invites
    /// </summary>
    protected readonly KirelInvitesOptions InvitesOptions;
    
    /// <summary>
    /// Service for invites user
    /// </summary>
    /// <param name="userManager">Identity user manager</param>
    /// <param name="mapper">Automapper instance</param>
    /// <param name="emailSender">IKirelEmailSender instance</param>
    /// <param name="smsSender">IKirelSmsSender instance</param>
    /// <param name="invitesOptions">KirelInvitesOptions</param>
    public KirelUserInviteService(UserManager<TUser> userManager, IMapper mapper, IKirelEmailSender emailSender, IKirelSmsSender smsSender, KirelInvitesOptions invitesOptions)
    {
        UserManager = userManager;
        Mapper = mapper;
        EmailSender = emailSender;
        SmsSender = smsSender;
        InvitesOptions = invitesOptions;
    }
   
    /// <summary>
    /// Invites user by email or sms
    /// </summary>
    /// <param name="userInviteDto">User invite dto type</param>
    /// <returns>Invited user</returns>
    public async Task<TUser> InviteUser(TUserInviteDto userInviteDto)
    {
        bool isEmail;
        string username;
        var user = Mapper.Map<TUser>(userInviteDto);
        if (user.PhoneNumber.IsNullOrEmpty() && user.Email != null)
        {
            username = userInviteDto.Email!.Split("@")[0];
            isEmail = true;
        }
        else if (user.Email.IsNullOrEmpty() && user.PhoneNumber != null)
        {
            username = "user-" + userInviteDto.PhoneNumber;
            isEmail = false;
        }
        else
        {
            throw new KirelValidationException("Only one of the Email or PhoneNumber fields must be fulfilled");
        }
        user.UserName = username;
        user.IsRegistrationFinished = false;
        var result = await UserManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            throw new KirelIdentityStoreException("Failed to create user");
        }
        await SendInvitation(user, isEmail);
        return user;
    }

    private async Task SendInvitation(TUser user, bool isEmail)
    {
        var query = $"?id={user.Id}";
        var link = InvitesOptions.ContinueRegistrationUrl + query;
        var text = $"You are invited to join CitMed academy. Please follow this link to register your account: {link}";
        if (isEmail)
            await EmailSender.SendEmailAsync("CitMed Academy", text, user.Email);
        else 
            await SmsSender.SendSms(text, user.PhoneNumber);
    }
}