using HR.LeaveManagement.Application.Contracts.Common;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Model.Identity;
using HR.LeaveManagement.Domain.Identity;
using HR.LeaveManagement.Identity.IdentityContext;
using HR.LeaveManagement.Identity.Seeding;
using HR.LeaveManagement.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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


        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

        var jwtSettings = new JwtSettings();
        configuration.Bind(nameof(JwtSettings), jwtSettings);


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
            };
        });




        services.AddTransient<IDbInitializer, IdentityDbInitializer>();

        services.AddHttpContextAccessor();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();


        return services;
    }
}
