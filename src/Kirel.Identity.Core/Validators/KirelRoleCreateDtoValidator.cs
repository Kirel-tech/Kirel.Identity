﻿using FluentValidation;
using Kirel.Identity.Core.Models;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Validators;

/// <summary>
/// Validation for KirelRoleCreateDto
/// </summary>
public class
    KirelRoleCreateDtoValidator<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim, TRoleCreateDto, TClaimCreateDto> : AbstractValidator<TRoleCreateDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
    where TRoleCreateDto : KirelRoleCreateDto<TClaimCreateDto>
    where TClaimCreateDto : KirelClaimCreateDto
    where TRoleClaim : KirelIdentityRoleClaim<TKey>
    where TUserClaim : KirelIdentityUserClaim<TKey>
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