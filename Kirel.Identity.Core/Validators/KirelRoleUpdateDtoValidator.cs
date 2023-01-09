using FluentValidation;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Validators;

/// <summary>
/// Validation for KirelRoleUpdateDto
/// </summary>
public class KirelRoleUpdateDtoValidator<TKey, TRole, TRoleUpdateDto, TClaimUpdateDto> : AbstractValidator<TRoleUpdateDto> 
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TRole : IdentityRole<TKey>
    where TRoleUpdateDto : KirelRoleUpdateDto<TClaimUpdateDto>
    where TClaimUpdateDto : KirelClaimUpdateDto
{
    private readonly RoleManager<TRole> _roleManager;

    /// <summary>
    /// Constructor for KirelRoleUpdateDtoValidator
    /// </summary>
    /// <param name="roleManager">Identity role manager</param>
    public KirelRoleUpdateDtoValidator(RoleManager<TRole> roleManager)
    {
        _roleManager = roleManager;
        var message = "";
        RuleFor(dto => dto.Name).Must((_, roleName) => RoleNameUnique(roleName, out message)).WithMessage(_ => message);
    }
    
    private bool RoleNameUnique(string roleName, out string errorMessage)
    {
        errorMessage = "";
        var unique = !_roleManager.Roles.Any(role => role.Name == roleName);
        if (unique) return true;
        errorMessage = "Role with given name already exists";
        return false;
    }
}