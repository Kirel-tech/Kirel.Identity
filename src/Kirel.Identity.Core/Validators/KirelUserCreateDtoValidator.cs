using FluentValidation;
using FluentValidation.Results;
using Kirel.Identity.Core.Models;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Kirel.Identity.Core.Validators;

/// <summary>
/// Validation for KirelUserCreateDto
/// </summary>
public class
    KirelUserCreateDtoValidator<TKey, TUser, TRole, TUserCreateDto, TClaimCreateDto> : AbstractValidator<TUserCreateDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey>
    where TRole : KirelIdentityRole<TKey>
    where TUserCreateDto : KirelUserCreateDto<TKey, TClaimCreateDto>
    where TClaimCreateDto : KirelClaimCreateDto
{
    private readonly RoleManager<TRole> _roleManager;
    private readonly UserManager<TUser> _userManager;

    /// <summary>
    /// Constructor for KirelUserCreateDto
    /// </summary>
    /// <param name="identityOptions"> Represents all the options you can use to configure the identity system </param>
    /// <param name="userManager"> Identity user manager </param>
    /// <param name="roleManager"> Identity role manager </param>
    public KirelUserCreateDtoValidator(IOptions<IdentityOptions> identityOptions, UserManager<TUser> userManager,
        RoleManager<TRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        var message = "";

        When(_ => identityOptions.Value.User.RequireUniqueEmail, () =>
        {
            RuleFor(dto => dto.Email)
                .EmailAddress().WithMessage("'Email' is an invalid email address.")
                .Must((dto, _) => EmailUnique(dto.Email, out message)).WithMessage(_ => message);
        });
        RuleFor(dto => dto.UserName)
            .MinimumLength(4).WithMessage("The username must be at least 4 characters long.")
            .Matches(@"^(?=.*[a-zA-Z]{1,})(?=.*[\d]{0,})[a-zA-Z0-9.]{4,20}$")
            .WithMessage("Username can only contains letters, numbers and dots")
            .Must((dto, _) => UserNameUnique(dto.UserName, out message)).WithMessage(_ => message);
        RuleFor(dto => dto.PhoneNumber)
            .Matches(@"(\d{1,3})?\d{3}?\d{3}?\d{4}").WithMessage("Enter a valid phone number." +
                                                                 " You need to transfer 10 digits and you can transfer the country code");
        RuleFor(dto => dto.Roles)
            .Must((_, roles) => RolesExist(roles, out message)).WithMessage(_ => message);
    }

    /// <inheritdoc />
    protected override bool PreValidate(ValidationContext<TUserCreateDto> context, ValidationResult result)
    {
        var passErrors = ValidatePassword(context.InstanceToValidate.Password);
        foreach (var error in passErrors) result.Errors.Add(new ValidationFailure("Password", error));
        return true;
    }


    private IList<string> ValidatePassword(string password)
    {
        var passErrors = new List<string>();
        foreach (var validator in _userManager.PasswordValidators)
        {
            var task = validator.ValidateAsync(_userManager, null, password);
            var result = task.Result;
            if (result.Succeeded) return passErrors;
            passErrors.AddRange(result.Errors.Select(error => error.Description));
        }

        return passErrors;
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