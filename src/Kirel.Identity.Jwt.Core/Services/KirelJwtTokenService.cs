using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
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
/// <typeparam name="TKey">User key type</typeparam>
/// <typeparam name="TUser">User type</typeparam>
/// <typeparam name="TRole">Role type</typeparam>
public class KirelJwtTokenService<TKey, TUser, TRole> 
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey> 
    where TUser : KirelIdentityUser<TKey>
    where TRole : KirelIdentityRole<TKey>
{
    /// <summary>
    /// Identity user manager
    /// </summary>
    protected readonly UserManager<TUser> UserManager;
    /// <summary>
    /// Identity role manager
    /// </summary>
    protected readonly RoleManager<TRole> RoleManager;
    /// <summary>
    /// Token auth options
    /// </summary>
    protected readonly KirelAuthOptions AuthOptions;

    /// <summary>
    /// KirelAuthenticationService constructor
    /// </summary>
    /// <param name="userManager">Identity user manager</param>
    /// <param name="roleManager">Identity role manager</param>
    /// <param name="authOptions">Token auth options</param>
    public KirelJwtTokenService(UserManager<TUser> userManager, RoleManager<TRole> roleManager, KirelAuthOptions authOptions)
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
        claims.Add(new Claim("uid", user.Id.ToString()!));
        foreach (var roleName in userRoles)
        {
            var role = await RoleManager.FindByNameAsync(roleName);
            var roleClaims = await RoleManager.GetClaimsAsync(role);
            
            foreach (var roleClaim in roleClaims)
                claims.Add(roleClaim);
            
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }
            
        return new ClaimsIdentity(claims, "JwtToken", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
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
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(AuthOptions.Key), SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private async Task<JwtTokenDto> GenerateTokensPair(TUser user)
    {
        var userClaims = await GetUserIdentityClaims(user);
        if (userClaims == null) throw new KirelAuthenticationException($"User with login {user.UserName} is not found");
        
        var accessToken = CreateJwtToken(userClaims, AuthOptions.AccessLifetime);

        var claimsList = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Role, "RefreshToken")
        };
        var refreshClaims = new ClaimsIdentity(claimsList, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        var refreshToken = CreateJwtToken(refreshClaims, AuthOptions.RefreshLifetime);

        return new JwtTokenDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    /// <summary>
    /// Method for getting JWT token DTO for authenticated user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public virtual async Task<JwtTokenDto> GenerateJwtTokenDto(TUser user)
    {
        return await GenerateTokensPair(user);
    }
}