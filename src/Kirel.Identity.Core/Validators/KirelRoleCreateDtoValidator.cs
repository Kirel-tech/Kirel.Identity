using FluentValidation;
using Kirel.Identity.Core.Models;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Validators;

/// <summary>
/// Validation for KirelRoleCreateDto
/// </summary>
public class
    KirelRoleCreateDtoValidator<TKey, TRole, TUser, TUserRole, TRoleCreateDto, TClaimCreateDto> : AbstractValidator<TRoleCreateDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole>
    where TRoleCreateDto : KirelRoleCreateDto<TClaimCreateDto>
    where TClaimCreateDto : KirelClaimCreateDto
{
    private readonly RoleManager<TRole> _roleManager;

    /// <summary>
    /// Constructor for KirelRoleCreateDtoValidator
    /// </summary>
    /// <param name="roleManager"> Identity role manager </param>
    public KirelRoleCreateDtoValidator(RoleManager<TRole> roleManager)
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