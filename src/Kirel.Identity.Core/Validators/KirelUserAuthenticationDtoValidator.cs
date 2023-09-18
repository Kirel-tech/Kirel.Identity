using FluentValidation;
using Kirel.Identity.Core.Models;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Validators;

/// <summary>
/// Validation for KirelUserAuthenticationDtoValidator
/// </summary>
public class KirelUserAuthenticationDtoValidator<TKey, TUser, TRole, TUserRole, TUserAuthenticationDto, TUserClaim, TRoleClaim> : AbstractValidator<TUserAuthenticationDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
    where TUserAuthenticationDto : KirelUserAuthenticationDto
    where TUserClaim : IdentityUserClaim<TKey>
    where TRoleClaim : IdentityRoleClaim<TKey>
{
    private readonly UserManager<TUser> _userManager;
    
    /// <summary>
    /// Constructor for KirelUserAuthenticationDtoValidator
    /// </summary>
    /// <param name="userManager">Identity user manager</param>
    public KirelUserAuthenticationDtoValidator(UserManager<TUser> userManager)
    {
        _userManager = userManager;
        
        var message = "";
        var typeValues = new List<string>{ "username", "phone", "email" };
        RuleFor(d => d.Type.ToLower())
            .Must(type => typeValues.Contains(type))
            .WithMessage($"Authentication type can only be one of the following: {string.Join(",", typeValues)}");
        When(d => d.Type.ToLower() == "username", () =>
        {
            RuleFor(d => d.Login)
                .MinimumLength(4).WithMessage("The username must be at least 4 characters long.")
                .Matches(@"^(?=.*[a-zA-Z]{1,})(?=.*[\d]{0,})[a-zA-Z0-9.]{4,20}$")
                .WithMessage("Username can only contains letters, numbers and dots");
        });
        When(d => d.Type.ToLower() == "phone", () =>
        {
            RuleFor(d => d.Login)
                .Matches("^[1-9][0-9]{10,12}$")
                .WithMessage("The phone number must be in international format: 11 to 13 digits without a plus")
                .Must((dto, _) => PhoneConfirmed(dto.Login, out message))
                .WithMessage(_ => message);
        });
        When(d => d.Type.ToLower() == "email", () =>
        {
            RuleFor(d => d.Login)
                .EmailAddress().WithMessage("The email address is invalid")
                .Must((dto, _) => EmailConfirmed(dto.Login, out message))
                .WithMessage(_ => message);
        });
    }
    
    private bool PhoneConfirmed(string phone, out string message)
    {
        message = "";
        var user = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == phone);
        if (user == null) return false;
        if (!user.PhoneNumberConfirmed)
            message = "To use phone authentication, you must confirm your phone number";
        return user.PhoneNumberConfirmed;
    }

    private bool EmailConfirmed(string email, out string message)
    {
        message = "";
        var user = _userManager.Users.FirstOrDefault(u => u.Email == email);
        if (user == null) return false;
        if (!user.EmailConfirmed)
            message = "To use email authentication, you must confirm your email";
        return user.EmailConfirmed;
    }
}