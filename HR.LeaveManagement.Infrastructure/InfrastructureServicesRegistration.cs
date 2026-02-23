using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Model;
using HR.LeaveManagement.Infrastructure.EmailService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HR.LeaveManagement.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BrevoEmailSettings>(configuration.GetSection("BrevoEmailSettings"));

        services.AddScoped<IEmailSender, BrevoEmailSender>();

        return services;
    }
}
