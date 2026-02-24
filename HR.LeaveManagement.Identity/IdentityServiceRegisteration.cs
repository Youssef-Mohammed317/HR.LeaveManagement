using HR.LeaveManagement.Application.Contracts.Common;
using HR.LeaveManagement.Domain.Identity;
using HR.LeaveManagement.Identity.IdentityContext;
using HR.LeaveManagement.Identity.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HR.LeaveManagement.Identity;

public static class IdentityServiceRegisteration
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HrIdentityDatabaseContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("HrIdentityDatabaseConnectionString"));
        });

        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<HrIdentityDatabaseContext>()
            .AddDefaultTokenProviders();

        services.AddTransient<IDbInitializer, IdentityDbInitializer>();
        return services;
    }
}
