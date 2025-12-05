using Microsoft.AspNetCore.Identity;
using UtilityManagementSystem.Models;

public static class IdentitySeed
{
    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "Admin", "Field Officer", "Cashier", "Manager" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    public static async Task SeedEmployees(UserManager<IdentityUser> userManager, UtilityDbContext db) { 
        var employees = db.Employees.ToList(); 
        foreach (var emp in employees) {
            if (await userManager.FindByNameAsync(emp.Username) == null) {
                var user = new IdentityUser { UserName = emp.Username, Email = emp.Email }; 
                await userManager.CreateAsync(user, "DefaultPassword123!"); 
                await userManager.AddToRoleAsync(user, emp.Role); 
            } 
        }
    }
}
