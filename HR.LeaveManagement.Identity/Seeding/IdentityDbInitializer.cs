using HR.LeaveManagement.Application.Contracts.Common;
using HR.LeaveManagement.Domain.Identity;
using HR.LeaveManagement.Domain.Utility;
using Microsoft.AspNetCore.Identity;

namespace HR.LeaveManagement.Identity.Seeding;

public class IdentityDbInitializer(UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager) : IDbInitializer
{
    public async Task InitializeAsync()
    {
        var employeeRole = Roles.Employee;
        var adminRole = Roles.Administrator;
        var adminEmail = "admin@test.com";
        var employeeEmail = "employee@test.com";
        var password = "Pa$$w0rd";

        if (!await roleManager.RoleExistsAsync(employeeRole))
            await roleManager.CreateAsync(new ApplicationRole { Name = employeeRole });

        if (!await roleManager.RoleExistsAsync(adminRole))
            await roleManager.CreateAsync(new ApplicationRole { Name = adminRole });

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                Email = adminEmail,
                FirstName = "System",
                LastName = "Admin",
                UserName = adminEmail,
                EmailConfirmed = true
            };


            await userManager.CreateAsync(admin, password);

            await userManager.AddToRoleAsync(admin, adminRole);

        }
        if (await userManager.FindByEmailAsync(employeeEmail) == null)
        {
            var employee = new ApplicationUser
            {
                Email = employeeEmail,
                FirstName = "System",
                LastName = "Employee",
                UserName = employeeEmail,
                EmailConfirmed = true
            };

            await userManager.CreateAsync(employee, password);

            await userManager.AddToRoleAsync(employee, employeeRole);
        }


    }
}
