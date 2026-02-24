using HR.LeaveManagement.API.Middlewares;
using HR.LeaveManagement.Application;
using HR.LeaveManagement.Identity;
using HR.LeaveManagement.Infrastructure;
using HR.LeaveManagement.Persistence;
namespace HR.LeaveManagement.API.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddPresentationServices(this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddControllers();
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPersistenceServices(configuration);
        services.AddApplicationServices(configuration);
        services.AddInfrastructureServices(configuration);
        services.AddIdentityServices(configuration);

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowAnyOrigin();
            });
        });

        services.AddTransient<GlobalExceptionHandlingMiddleware>();

        return services;
    }
}
