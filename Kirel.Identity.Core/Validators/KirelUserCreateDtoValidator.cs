using FluentValidation;
using Kirel.Identity.DTOs;
using Kirel.Identity.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Validators;

/// <summary>
/// Validation for KirelUserCreateDto
/// </summary>
public class KirelUserCreateDtoValidator<TKey, TUser, TRole, TUserCreateDto, TClaimCreateDto> : AbstractValidator<TUserCreateDto> 
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : IdentityUser<TKey>, IKirelUser<TKey> 
    where TRole : IdentityRole<TKey>
    where TUserCreateDto : KirelUserCreateDto<TKey, TClaimCreateDto>
    where TClaimCreateDto : KirelClaimCreateDto
{
    private readonly UserManager<TUser> _userManager;
    private readonly RoleManager<TRole> _roleManager;

    /// <summary>
    /// Constructor for KirelUserCreateDto
    /// </summary>
    /// <param name="userManager">Identity user manager</param>
    /// <param name="roleManager">Identity role manager</param>
    public KirelUserCreateDtoValidator(UserManager<TUser> userManager, RoleManager<TRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        var message = "";

        RuleFor(dto => dto.Password)
            .MinimumLength(8).WithMessage("Your password length must be at least 8.")                    
            .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");
        RuleFor(dto => dto.UserName)
            .MinimumLength(4).WithMessage("The username must be at least 4 characters long.")
            .Matches(@"^(?=.*[a-zA-Z]{1,})(?=.*[\d]{0,})[a-zA-Z0-9.]{4,20}$").WithMessage("Username can only contains letters, numbers and dots")
            .Must((dto, _) => UserNameUnique(dto.UserName, out message)).WithMessage(_ => message);
        RuleFor(dto => dto.Email)
            .Matches(@"^[\w._-]{4,}@+\w{2,}(.com|.co|.uk|.ru)$").WithMessage("Email must be abc@def.com|ru")
            .Must((dto, _) => EmailUnique(dto.Email, out message)).WithMessage(_ => message);
        RuleFor(dto => dto.PhoneNumber)
            .Matches(@"(\d{1,3})?\d{3}?\d{3}?\d{4}").WithMessage("Enter a valid phone number." +
                " You need to transfer 10 digits and you can transfer the country code");
        RuleFor(dto => dto.Roles)
            .Must((_, roles) => RolesExist(roles, out message)).WithMessage(_ => message);
    }

    private bool RolesExist(List<TKey> roles, out string errorMessage)
    {
        errorMessage = "";
        var exists = _roleManager.Roles.Count(role => roles.Contains(role.Id)) == roles.Count;
        if (exists) return true;
        errorMessage = "One of the passed roles does not exist";
        return false;
    }

    private bool UserNameUnique(string userName, out string errorMessage)
    {
        errorMessage = "";
        var unique = !_userManager.Users.Any(user => user.UserName == userName);
        if (unique) return true;
        errorMessage = $"This username {userName} is already taken";
        return false;
    }

    private bool EmailUnique(string email, out string errorMessage)
    {
        errorMessage = "";
        var unique = !_userManager.Users.Any(user => user.Email == email);
        if (unique) return true;
        errorMessage = $"This email {email} is already taken";
        return false;
    }
}