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
    public static async Task UsersAndRolesDataSeedAsync<TKey, TUser, TRole, TUserRole>(this IApplicationBuilder app, IdentityDataSeedConfig config)
        where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole>, new ()
        where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole>, new ()
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole>
    {
        const string dataSeedLockfile = "identity_data_seed.lock";
        if (File.Exists(dataSeedLockfile)) return;
        var scopedServiceProvider = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope()
            .ServiceProvider;
        var roleManager = scopedServiceProvider.GetRequiredService<RoleManager<TRole>>();
        var userManager = scopedServiceProvider.GetRequiredService<UserManager<TUser>>();
        
        foreach (var roleName in config.Roles.Where(r => !r.Equals("Admin", StringComparison.OrdinalIgnoreCase)))
        {
            var roleNormalizedName = roleName.ToUpper();

            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (roleExist) continue;
            var role = new TRole { Name = roleName, NormalizedName = roleNormalizedName };
            var result = await roleManager.CreateAsync(role);
            if (result.Succeeded) continue;
            var errorMessage = $"Error creating role {roleName}: ";
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
                LastName = userConfig.LastName
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