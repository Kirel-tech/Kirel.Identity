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
/// <typeparam name="TUserClaim"> User claim type. </typeparam>
/// <typeparam name="TRoleClaim"> Role claim type. </typeparam>
public class KirelJwtTokenService<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>
    where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>
    where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
    where TRoleClaim : KirelIdentityRoleClaim<TKey>
    where TUserClaim : KirelIdentityUserClaim<TKey>
{
    /// <summary>
    /// Token auth options
    /// </summary>
    protected readonly KirelAuthOptions AuthOptions;

    /// <summary>
    /// KirelAuthenticationService constructor
    /// </summary>
    /// <param name="authOptions"> Token auth options </param>
    public KirelJwtTokenService(KirelAuthOptions authOptions)
    {
        AuthOptions = authOptions;
    }

    private Task<ClaimsIdentity> GetUserIdentityClaims(TUser user)
    {
        var claims = user.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();

        claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iss, AuthOptions.Issuer));
        claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.UserName));
        claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()!));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        foreach (var role in user.UserRoles.Select(ur => ur.Role))
        {
            claims.AddRange(role.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)));
            claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name));
            claims.Add(new Claim("role", role.Name));
        }
        return Task.FromResult(new ClaimsIdentity(claims, "JwtToken", ClaimsIdentity.DefaultNameClaimType,
        ClaimsIdentity.DefaultRoleClaimType));
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
            new(JwtRegisteredClaimNames.Iss, AuthOptions.Issuer),
            new(JwtRegisteredClaimNames.NameId, user.Id.ToString()!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
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