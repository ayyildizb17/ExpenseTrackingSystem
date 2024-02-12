using ExpenseTrackingSystem.Entities.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

public static class RoleInitializer
{
    public const string SysAdminRoleName = "SysAdmin";
    public const string ManagerRoleName = "Manager";
    public const string EmployeeRoleName = "Employee";

    public static async Task InitializeRoles(RoleManager<CustomUserRole> roleManager)
    {
        string[] roleNames = { SysAdminRoleName, ManagerRoleName, EmployeeRoleName };

        foreach (var roleName in roleNames)
        {
            
                await roleManager.CreateAsync(new CustomUserRole { Name = roleName , NormalizedName = roleName, Id = Guid.NewGuid().ToString() });
            
        }
    }
    public static async Task InitializeAsync(UserManager<CustomUser> userManager, RoleManager<CustomUserRole> roleManager)
    {
        string adminEmail = "admin";
        string password = "Admin_12";

        await InitializeRoles(roleManager);


        if (await roleManager.FindByNameAsync("SysAdmin") == null)
        {
           
            await roleManager.CreateAsync(new CustomUserRole { Name = "SysAdmin", Id = Guid.NewGuid().ToString() });
        }

        // Create admin user
        if (await userManager.FindByNameAsync(adminEmail) == null)
        {
            var sysAdminRole = await roleManager.FindByNameAsync("SysAdmin");
            CustomUser admin = new CustomUser { UserName = adminEmail , Id = Guid.NewGuid().ToString(), RoleId = sysAdminRole.Id};
            IdentityResult result = await userManager.CreateAsync(admin, password);

            if (result.Succeeded)
            {
                
                await userManager.AddToRoleAsync(admin, "SysAdmin");
            }
        }
    }

}

