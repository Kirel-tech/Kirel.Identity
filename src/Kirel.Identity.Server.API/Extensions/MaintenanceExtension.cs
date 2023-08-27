using Kirel.Identity.Core.Models;
using Kirel.Identity.Server.Domain;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Server.API.Extensions;

/// <summary>
/// Maintenance application builder extension
/// </summary>
public static class MaintenanceExtension
{
    /// <summary>
    /// Do user data maintenance
    /// </summary>
    /// <param name="app"> Application builder </param>
    /// <param name="config"> Maintenance config </param>
    /// <typeparam name="TKey"> User entity key type. </typeparam>
    /// <typeparam name="TUser"> User entity type. </typeparam>
    /// <typeparam name="TRole"> Role entity type. </typeparam>
    /// <typeparam name="TUserRole"> Role user entity type. </typeparam>
    public static async Task MaintenanceAsync<TKey, TUser, TRole, TUserRole>(
        this IApplicationBuilder app, MaintenanceConfig config)
        where TRole : KirelIdentityRole<TKey, TRole, TUser, TUserRole>, new ()
        where TUser : KirelIdentityUser<TKey, TUser, TRole, TUserRole>, new ()
        where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
        where TUserRole : KirelIdentityUserRole<TKey, TUserRole, TUser, TRole>
    {
        var scopedServiceProvider = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope()
            .ServiceProvider;
        var roleManager = scopedServiceProvider.GetRequiredService<RoleManager<TRole>>();
        var userManager = scopedServiceProvider.GetRequiredService<UserManager<TUser>>();
        var adminRoleExist = true;
        var adminRole = await roleManager.FindByNameAsync("Admin");
        if (adminRole == null)
        {
            adminRole = new TRole { Name = "Admin", NormalizedName = "ADMIN" };
            var result = await roleManager.CreateAsync(adminRole);
            if (!result.Succeeded)
                adminRoleExist = false;
        }

        var adminUserExist = true;
        var adminUser = await userManager.FindByNameAsync("Admin");
        if (adminUser == null)
        {
            adminUser = new TUser
            {
                UserName = "Admin", Email = "admin@citmed.ru", Name = "Admin", LastName = "Default"
            };
            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (!result.Succeeded)
                adminUserExist = false;
        }

        if (adminUserExist && config.Admin.Reset)
        {
            var newPassword = config.Admin.Password != "" ? config.Admin.Password : "Admin@123";
            await userManager.RemovePasswordAsync(adminUser);
            await userManager.AddPasswordAsync(adminUser, newPassword);
        }

        if (!adminUserExist || !adminRoleExist) return;
        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}