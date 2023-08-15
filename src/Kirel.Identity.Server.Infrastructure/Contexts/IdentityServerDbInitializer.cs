using Kirel.Identity.Server.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kirel.Identity.Server.Infrastructure.Contexts
{
    /// <summary>
    /// Class responsible for initializing the Identity Server database
    /// </summary>
    public static class IdentityServerDbInitializer
    {
        /// <summary>
        /// Initializes the Identity Server database with roles and users.
        /// </summary>
        /// <typeparam name="TDbContext">The type of the database context.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="maintenanceConfig">The maintenance configuration.</param>
        public static async Task Initialize<TDbContext>(IServiceProvider serviceProvider, MaintenanceConfig maintenanceConfig)
            where TDbContext : DbContext
        {
            var dataContext = serviceProvider.GetRequiredService<TDbContext>();
            await dataContext.Database.MigrateAsync();
            
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            if (maintenanceConfig?.Admin?.Reset == true)
            {
                var adminUser = await userManager.FindByNameAsync("Admin");
                var newPassword = maintenanceConfig.Admin.Password ?? "Admin@123";
                await userManager.RemovePasswordAsync(adminUser);
                await userManager.AddPasswordAsync(adminUser, newPassword);
            }

            foreach (var roleConfig in maintenanceConfig!.Roles)
            {
                var roleName = roleConfig.Name;
                var roleNormalizedName = roleConfig.NormalizedName;

                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var role = new Role { Name = roleName, NormalizedName = roleNormalizedName };
                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        
                    }
                }
            }

            foreach (var userConfig in maintenanceConfig.Users)
            {
                var userName = userConfig.UserName;
                var email = userConfig.Email;
                var name = userConfig.Name;
                var lastName = userConfig.LastName;
                var password = userConfig.Password;

                var userExist = await userManager.FindByNameAsync(userName);
                if (userExist == null)
                {
                    var user = new User
                    {
                        UserName = userName,
                        Email = email,
                        Name = name,
                        LastName = lastName
                    };
                    var result = await userManager.CreateAsync(user, password);
                    if (!result.Succeeded)
                    {
                        
                    }

                    if (userConfig.Roles != null)
                        foreach (var roleName in userConfig.Roles)
                        {
                            if (await roleManager.RoleExistsAsync(roleName))
                            {
                                await userManager.AddToRoleAsync(user, roleName);
                            }
                        }
                }
            }

            
        }
    }
}
