using Kirel.Identity.Core.Models;
using Kirel.Identity.Server.API.Configs;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Server.API.Extensions;

/// <summary>
/// Identity users and roles data seed application builder extension
/// </summary>
public static class UsersAndRolesDataSeedExtension
{
    /// <summary>
    /// Apply identity data seed configuration
    /// </summary>
    /// <param name="app"> Application builder </param>
    /// <param name="config"> Maintenance config </param>
    /// <typeparam name="TKey"> User entity key type. </typeparam>
    /// <typeparam name="TUser"> User entity type. </typeparam>
    /// <typeparam name="TRole"> Role entity type. </typeparam>
    /// <typeparam name="TUserRole"> Role user entity type. </typeparam>
    /// <typeparam name="TUserClaim"> User claim type. </typeparam>
    /// <typeparam name="TRoleClaim"> Role claim type. </typeparam>
    public static async Task UsersAndRolesDataSeedAsync<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>(this IApplicationBuilder app, IdentityDataSeedConfig config)
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole, TUserClaim, TRoleClaim>, new()
        where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole, TRoleClaim, TUserClaim>, new()
        where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole, TUserClaim, TRoleClaim>
        where TRoleClaim : KirelIdentityRoleClaim<TKey>, new()
        where TUserClaim : KirelIdentityUserClaim<TKey>, new()
    {
        const string dataSeedLockfile = "identity_data_seed.lock";
        if (File.Exists(dataSeedLockfile)) return;
        var scopedServiceProvider = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope()
            .ServiceProvider;
        var roleManager = scopedServiceProvider.GetRequiredService<RoleManager<TRole>>();
        var userManager = scopedServiceProvider.GetRequiredService<UserManager<TUser>>();
        
        foreach (var roleConfig in config.Roles.Where(r => !r.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase)))
        {
            var roleExist = await roleManager.RoleExistsAsync(roleConfig.Name);
            if (roleExist) continue;
            var role = new TRole
            {
                Name = roleConfig.Name, 
                NormalizedName = roleConfig.Name.ToUpper(),
                Claims = roleConfig.Claims.Select(c => new TRoleClaim()
                {
                    ClaimValue = c.Value,
                    ClaimType = c.Type
                }).ToList()
            };
            var result = await roleManager.CreateAsync(role);
            if (result.Succeeded) continue;
            var errorMessage = $"Error creating role {roleConfig.Name}: ";
            foreach (var error in result.Errors)
            {
                errorMessage += $"{error.Description}";
                if (result.Errors.Last() != error)
                    errorMessage += ", ";
            }
            throw new Exception(errorMessage);
        }

        foreach (var userConfig in config.Users.Where(u => !u.UserName.Equals("Admin", StringComparison.OrdinalIgnoreCase)))
        {
            var user = await userManager.FindByNameAsync(userConfig.UserName);
            if (user != null) continue;
            user = new TUser
            {
                UserName = userConfig.UserName,
                Email = userConfig.Email,
                Name = userConfig.Name,
                LastName = userConfig.LastName,
                Claims = userConfig.Claims.Select(c => new TUserClaim()
                {
                    ClaimType = c.Type,
                    ClaimValue = c.Value
                }).ToList()
            };
            var result = await userManager.CreateAsync(user, userConfig.Password);
            if (!result.Succeeded)
            {
                var errorMessage = $"Error creating user {userConfig.UserName}: ";
                foreach (var error in result.Errors)
                {
                    errorMessage += $"{error.Description}";
                    if (result.Errors.Last() != error)
                        errorMessage += ", ";
                }
                throw new Exception(errorMessage);
            }

            if (userConfig.Roles == null || userConfig.Roles.Count < 1) continue;
            foreach (var roleName in userConfig.Roles)
            {
                if (await roleManager.RoleExistsAsync(roleName))
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
        await File.Create(dataSeedLockfile).DisposeAsync();
    }
}