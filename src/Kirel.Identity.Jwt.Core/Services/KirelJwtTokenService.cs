using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Kirel.Identity.Core.Models;
using Kirel.Identity.Exceptions;
using Kirel.Identity.Jwt.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Kirel.Identity.Jwt.Core.Services;

/// <summary>
/// Provides a set of methods for generating JWT token and JWT Refresh tokens for the user
/// </summary>
/// <typeparam name="TKey"> User key type </typeparam>
/// <typeparam name="TUser"> User type </typeparam>
/// <typeparam name="TRole"> Role type </typeparam>
/// <typeparam name="TUserRole"> User role entity type </typeparam>
public class KirelJwtTokenService<TKey, TUser, TRole, TUserRole>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole>
{
    /// <summary>
    /// Token auth options
    /// </summary>
    protected readonly KirelAuthOptions AuthOptions;

    /// <summary>
    /// Identity role manager
    /// </summary>
    protected readonly RoleManager<TRole> RoleManager;

    /// <summary>
    /// Identity user manager
    /// </summary>
    protected readonly UserManager<TUser> UserManager;

    /// <summary>
    /// KirelAuthenticationService constructor
    /// </summary>
    /// <param name="userManager"> Identity user manager </param>
    /// <param name="roleManager"> Identity role manager </param>
    /// <param name="authOptions"> Token auth options </param>
    public KirelJwtTokenService(UserManager<TUser> userManager, RoleManager<TRole> roleManager,
        KirelAuthOptions authOptions)
    {
        UserManager = userManager;
        RoleManager = roleManager;
        AuthOptions = authOptions;
    }

    private async Task<ClaimsIdentity> GetUserIdentityClaims(TUser user)
    {
        var claims = await UserManager.GetClaimsAsync(user);
        var userRoles = await UserManager.GetRolesAsync(user);

        claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iss, AuthOptions.Issuer));
        claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.UserName));
        claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()!));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        foreach (var roleName in userRoles)
        {
            var role = await RoleManager.FindByNameAsync(roleName);
            var roleClaims = await RoleManager.GetClaimsAsync(role);

            foreach (var roleClaim in roleClaims)
                claims.Add(roleClaim);

            claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name));
        }

        return new ClaimsIdentity(claims, "JwtToken", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
    }

    private string CreateJwtToken(ClaimsIdentity claims, int lifetime)
    {
        var now = DateTime.UtcNow;
        var lifeTime = Convert.ToDouble(lifetime);
        var jwt = new JwtSecurityToken(
            AuthOptions.Issuer,
            AuthOptions.Audience,
            notBefore: now,
            claims: claims.Claims,
            expires: now.AddMinutes(lifeTime),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(AuthOptions.Key),
                SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private async Task<JwtTokenDto> GenerateTokensPair(TUser user)
    {
        var userClaims = await GetUserIdentityClaims(user);
        if (userClaims == null) throw new KirelAuthenticationException($"User with login {user.UserName} is not found");

        var accessToken = CreateJwtToken(userClaims, AuthOptions.AccessLifetime);

        var claimsList = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, user.UserName),
            new(ClaimsIdentity.DefaultRoleClaimType, "RefreshToken"),
            new(JwtRegisteredClaimNames.Name, user.UserName),
            new(JwtRegisteredClaimNames.NameId, user.Id.ToString()!)
        };
        var refreshClaims = new ClaimsIdentity(claimsList, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        var refreshToken = CreateJwtToken(refreshClaims, AuthOptions.RefreshLifetime);

        return new JwtTokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    /// <summary>
    /// Method for getting JWT token DTO for authenticated user
    /// </summary>
    /// <param name="user"> </param>
    /// <returns> </returns>
    public virtual async Task<JwtTokenDto> GenerateJwtTokenDto(TUser user)
    {
        return await GenerateTokensPair(user);
    }
}