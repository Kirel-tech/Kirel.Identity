using Kirel.Identity.Server.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kirel.Identity.Server.Infrastructure.Contexts;

/// <summary>
/// Database context initializer
/// </summary>
public static class IdentityServerDbInitializer
{
    /// <summary>
    /// Method for data initialisation
    /// </summary>
    /// <param name="serviceProvider"> Parameter of IServiceProvider </param>
    /// <param name="maintenance"> Config maintenance </param>
    public static async Task Initialize<TDbContext>(IServiceProvider serviceProvider, MaintenanceConfig maintenance)
        where TDbContext : DbContext
    {
        var dataContext = serviceProvider.GetRequiredService<TDbContext>();
        await dataContext.Database.MigrateAsync();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var adminRoleExist = true;
        var adminRole = await roleManager.FindByNameAsync("Admin");
        if (adminRole == null)
        {
            adminRole = new Role { Name = "Admin", NormalizedName = "ADMIN" };
            var result = await roleManager.CreateAsync(adminRole);
            if (!result.Succeeded)
                adminRoleExist = false;
        }

        var coursesManagerRoleExist = true;
        var coursesManagerRole = await roleManager.FindByNameAsync("Courses.Manager");
        if (coursesManagerRole == null)
        {
            coursesManagerRole = new Role { Name = "Courses.Manager", NormalizedName = "COURSES.MANAGER" };
            var result = await roleManager.CreateAsync(coursesManagerRole);
            if (!result.Succeeded)
                coursesManagerRoleExist = false;
        }

        var adminUserExist = true;
        var adminUser = await userManager.FindByNameAsync("Admin");
        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = "Admin", Email = "admin@citmed.ru", Name = "Admin", LastName = "Default"
            };
            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (!result.Succeeded)
                adminUserExist = false;
        }

        if (adminUserExist && maintenance.Admin.Reset)
        {
            var newPassword = maintenance.Admin.Password != "" ? maintenance.Admin.Password : "Admin@123";
            await userManager.RemovePasswordAsync(adminUser);
            await userManager.AddPasswordAsync(adminUser, newPassword);
        }

        if (!adminUserExist || !adminRoleExist) return;
        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            await userManager.AddToRoleAsync(adminUser, "Admin");

        if (!adminUserExist || !coursesManagerRoleExist) return;
        if (!await userManager.IsInRoleAsync(adminUser, "Courses.Manager"))
            await userManager.AddToRoleAsync(adminUser, "Courses.Manager");
    }
}