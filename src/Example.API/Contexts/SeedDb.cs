using Example.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Example.API.Contexts;

/// <summary>
/// Static class for database initialization
/// </summary>
public static class SeedDb
{
    private static RoleManager<ExRole> _roleManager;
    private static UserManager<ExUser>_userManager;
    private static async Task<ExRole> FindOrCreateRole(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role != null) return role;

        role = new ExRole() { Name = roleName };
        var result = await _roleManager.CreateAsync(role);
        return !result.Succeeded ? null : role;
    }
    private static async Task<ExUser> FindOrCreateUser(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user != null) return user;
        
        user = new ExUser()
        {
            UserName = username, Email = $"{username}@kirel.com", Name = username, LastName = "Default"
        };
        var result = await _userManager.CreateAsync(user, $"{username}@123");
        return !result.Succeeded ? null : user;
    }

    private static async Task AddUserToRoleIfNotAlreadyInRole(ExUser user, string roleName)
    {
        if(!await _userManager.IsInRoleAsync(user, roleName))
            await _userManager.AddToRoleAsync(user, roleName);
    }

    /// <summary>
    /// Database initialization
    /// </summary>
    /// <param name="serviceProvider">Service provider</param>
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ExKirelIdentityContext>();
        context.Database.EnsureCreated();
        
        _roleManager = serviceProvider.GetRequiredService<RoleManager<ExRole>>();
        _userManager = serviceProvider.GetRequiredService<UserManager<ExUser>>();
        
        var adminUser = await FindOrCreateUser("Admin");
        var adminRole = await FindOrCreateRole("Admin");
        var userRole = await FindOrCreateRole("User");
        var userUser = await FindOrCreateUser("User");
        
        if (adminRole != null && adminUser != null)
        {
            await AddUserToRoleIfNotAlreadyInRole(adminUser, "Admin");
        }

        if (userRole != null && adminUser != null)
        {
            await AddUserToRoleIfNotAlreadyInRole(adminUser, "User");
        }

        if (userRole != null && userUser != null)
        {
            await AddUserToRoleIfNotAlreadyInRole(userUser, "User");
        }
    }
}