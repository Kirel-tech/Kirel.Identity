using System.Text.RegularExpressions;
using FluentValidation;
using Kirel.Identity.Core.Models;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Core.Validators;

/// <summary>
/// Validation for KirelRoleUpdateDto
/// </summary>
public class
    KirelRoleUpdateDtoValidator<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim, TRoleUpdateDto, TClaimUpdateDto> : AbstractValidator<TRoleUpdateDto>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
    where TRoleUpdateDto : KirelRoleUpdateDto<TClaimUpdateDto>
    where TClaimUpdateDto : KirelClaimUpdateDto
    where TRoleClaim : KirelIdentityRoleClaim<TKey>
    where TUserClaim : KirelIdentityUserClaim<TKey>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly RoleManager<TRole> _roleManager;

    /// <summary>
    /// Constructor for KirelRoleUpdateDtoValidator
    /// </summary>
    /// <param name="roleManager"> Identity role manager </param>
    /// <param name="httpContextAccessor"> Http context accessor used for getting path </param>
    public KirelRoleUpdateDtoValidator(RoleManager<TRole> roleManager, IHttpContextAccessor httpContextAccessor)
    {
        _roleManager = roleManager;
        _httpContextAccessor = httpContextAccessor;
        var message = "";
        RuleFor(dto => dto.Name).Must((_, roleName) => RoleNameUnique(roleName, out message)).WithMessage(_ => message);
    }

    private bool RoleNameUnique(string roleName, out string errorMessage)
    {
        errorMessage = "";
        var path = _httpContextAccessor.HttpContext.Request.Path.Value;
        var regex = new Regex("roles/([0-9A-Za-z-]*)");
        var match = regex.Match(path);
        if (!match.Success) return false;
        var roleIdStr = match.Groups[1].Value;
        var role = _roleManager.FindByIdAsync(roleIdStr).Result;
        errorMessage = "";
        var unique = !_roleManager.Roles.Any(r => r.Name == roleName && !role.Id.Equals(r.Id));
        if (unique) return true;
        errorMessage = "Role with given name already exists";
        return false;
    }
}