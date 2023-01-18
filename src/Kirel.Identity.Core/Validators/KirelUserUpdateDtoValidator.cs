using System.Text.RegularExpressions;
using FluentValidation;
using Kirel.Identity.DTOs;
using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace Kirel.Identity.Core.Validators;

/// <summary>
/// Validation class for user update dto
/// </summary>
public class KirelUserUpdateDtoValidator<TKey, TUser, TRole, TUserUpdateDto, TClaimUpdateDto> : AbstractValidator<TUserUpdateDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey>
    where TRole : KirelIdentityRole<TKey>
    where TUserUpdateDto : KirelUserUpdateDto<TKey, TClaimUpdateDto>
    where TClaimUpdateDto : KirelClaimUpdateDto
{
    private readonly UserManager<TUser> _userManager;
    private readonly RoleManager<TRole> _roleManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    /// <summary>
    /// Constructor for UserUpdateDtoValidator
    /// </summary>
    /// <param name="userManager">Identity user manager</param>
    /// <param name="roleManager">Identity role manager</param>
    /// <param name="httpContextAccessor">Http context accessor used for getting path</param>
    public KirelUserUpdateDtoValidator(UserManager<TUser> userManager, RoleManager<TRole> roleManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _httpContextAccessor = httpContextAccessor;
        
        var message = "";

        When(dto => !string.IsNullOrEmpty(dto.Password), () =>
        {
            RuleFor(dto => dto.Password)
                .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");
        });
        RuleFor(dto => dto.Email)
            .EmailAddress().WithMessage("'Email' is an invalid email address.")
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

    private bool EmailUnique(string email, out string errorMessage)
    {
        errorMessage = "";
        var path = _httpContextAccessor.HttpContext.Request.Path.Value;
        var regex = new Regex("users/([0-9A-Za-z-]*)");
        var match = regex.Match(path);
        if (!match.Success) return false;
        var userId = match.Groups[1].Value;
        var user = _userManager.FindByIdAsync(userId).Result;
        var unique = !_userManager.Users.Any(u => u.Email == email && !u.Id.Equals(user.Id));
        if (unique) return true;
        errorMessage = $"This email {email} is already taken";
        return false;
    }
}