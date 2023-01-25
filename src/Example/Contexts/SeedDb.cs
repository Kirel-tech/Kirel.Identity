using Example.Models;
using Microsoft.AspNetCore.Identity;

namespace Example.Contexts;

/// <summary>
/// Static class for database initialization
/// </summary>
public static class SeedDb
{
    /// <summary>
    /// Database initialization
    /// </summary>
    /// <param name="serviceProvider">Service provider</param>
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ExKirelIdentityContext>();
        context.Database.EnsureCreated();
        
        var adminRoleExist = true;
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ExRole>>();
        var adminRole = await roleManager.FindByNameAsync("Admin");
        if (adminRole == null)
        {
            adminRole = new ExRole(){ Name = "Admin", NormalizedName = "ADMIN" };
            var result = await roleManager.CreateAsync(adminRole);
            if (!result.Succeeded)
                adminRoleExist = false;
        }
        var userRoleExist = true;
        var userRole = await roleManager.FindByNameAsync("User");
        if (userRole == null)
        {
            userRole = new ExRole(){ Name = "User", NormalizedName = "USER" };
            var result = await roleManager.CreateAsync(userRole);
            if (!result.Succeeded)
                userRoleExist = false;
        }

        var adminUserExist = true;
        var userManager = serviceProvider.GetRequiredService<UserManager<ExUser>>();
        var adminUser = await userManager.FindByNameAsync("Admin");
        if (adminUser == null)
        {
            adminUser = new ExUser()
            {
                UserName = "Admin", Email = "admin@kirel.com", Name = "Admin", LastName = "Default"
            };
            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (!result.Succeeded)
                adminUserExist = false;
        }
        
        var userUserExist = true;
        var userUser = await userManager.FindByNameAsync("User");
        if (userUser == null)
        {
            userUser = new ExUser()
            {
                UserName = "User", Email = "user@kirel.com", Name = "User", LastName = "Default"
            };
            var result = await userManager.CreateAsync(userUser, "User@123");
            if (!result.Succeeded)
                userUserExist = false;
        }

        if (adminUserExist && adminRoleExist)
        {
            if(!await userManager.IsInRoleAsync(adminUser, "Admin"))
                await userManager.AddToRoleAsync(adminUser, "Admin");
        }
        
        if (adminUserExist && userRoleExist)
        {
            if(!await userManager.IsInRoleAsync(adminUser, "User"))
                await userManager.AddToRoleAsync(adminUser, "User");
        }
        
        if (userUserExist && userRoleExist)
        {
            if(!await userManager.IsInRoleAsync(userUser, "User"))
                await userManager.AddToRoleAsync(userUser, "User");
        }
    }
}