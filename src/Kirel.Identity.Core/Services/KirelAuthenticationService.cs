using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using AutoMapper;
using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Models;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Kirel.Identity.Core.Services;

/// <summary>
/// Service for validating user credentials and issuing tokens
/// </summary>
/// <typeparam name="TKey">User key type</typeparam>
/// <typeparam name="TUser">User type</typeparam>
/// <typeparam name="TRegisterDto">User register dto type</typeparam>
public class KirelAuthenticationService<TKey, TUser, TRegisterDto> 
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey> 
    where TUser : KirelIdentityUser<TKey>
    where TRegisterDto : KirelUserRegisterDto
{
    /// <summary>
    /// Identity user manager
    /// </summary>
    protected readonly UserManager<TUser> UserManager;
    /// <summary>
    /// Identity role manager
    /// </summary>
    protected readonly RoleManager<IdentityRole<TKey>> RoleManager;
    /// <summary>
    /// Token auth options
    /// </summary>
    protected readonly KirelAuthOptions AuthOptions;
    /// <summary>
    /// AutoMapper instance
    /// </summary>
    protected readonly IMapper Mapper;

    /// <summary>
    /// KirelAuthenticationService constructor
    /// </summary>
    /// <param name="userManager">Identity user manager</param>
    /// <param name="roleManager">Identity role manager</param>
    /// <param name="authOptions">Token auth options</param>
    /// <param name="mapper">AutoMapper instance</param>
    public KirelAuthenticationService(UserManager<TUser> userManager, RoleManager<IdentityRole<TKey>> roleManager, KirelAuthOptions authOptions, IMapper mapper)
    {
        UserManager = userManager;
        RoleManager = roleManager;
        AuthOptions = authOptions;
        Mapper = mapper;
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
            
        return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
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
    
    /// <summary>
    /// Method for add new user to identity db
    /// </summary>
    /// <param name="createDto">RegistrationRequestDto</param>
    /// <returns>New application user</returns>
    public virtual async Task Register(TRegisterDto createDto)
    {
        var appUser = Mapper.Map<TUser>(createDto);
        var result = await UserManager.CreateAsync(appUser);
        if(!result.Succeeded)  throw new AuthenticationException("Failed to create new user");
        var passwordResult = await UserManager.AddPasswordAsync(appUser, createDto.Password);
        if (!passwordResult.Succeeded)
        {
            await UserManager.DeleteAsync(appUser);
            throw new AuthenticationException("Failed to add password");
        }
    }

    private async Task<JwtTokenDto> GenerateTokensPair(TUser user)
    {
        var userClaims = await GetUserIdentityClaims(user);
        if (userClaims == null) throw new AuthenticationException($"User with login {user.UserName} is not found");
        
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
    /// Method for getting JWT token
    /// </summary>
    /// <param name="login">User login</param>
    /// <param name="password">User password</param>
    public virtual async Task<JwtTokenDto> GetJwtToken(string login, string password)
    {
        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password)) 
            throw new AuthenticationException("Login or password cannot be empty");
        var user = await UserManager.FindByNameAsync(login);
        if (user == null) throw new AuthenticationException($"User with login {login} is not found");
        var result = await UserManager.CheckPasswordAsync(user, password);
        if (!result) throw new AuthenticationException("Wrong password");
        return await GenerateTokensPair(user);
    }

    /// <summary>
    /// Method for refresh JWT token
    /// </summary>
    /// <param name="login">User login</param>
    /// <returns>JWT token dto</returns>
    /// <exception cref="AuthenticationException">If user with given login was not found</exception>
    public virtual async Task<JwtTokenDto> RefreshToken(string login)
    {
        var user = await UserManager.FindByNameAsync(login);
        if (user == null) throw new AuthenticationException($"User with login {login} is not found");
        return await GenerateTokensPair(user);
    }
}