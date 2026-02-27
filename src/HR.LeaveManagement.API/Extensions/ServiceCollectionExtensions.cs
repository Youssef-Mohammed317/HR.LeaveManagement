using HR.LeaveManagement.API.Middlewares;
using HR.LeaveManagement.Application;
using HR.LeaveManagement.Identity;
using HR.LeaveManagement.Infrastructure;
using HR.LeaveManagement.Persistence;
using Microsoft.OpenApi;
namespace HR.LeaveManagement.API.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddPresentationServices(this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddControllers();
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            });
            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("bearer", document)] = []
            });
        });

        return services;
    }
    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPresentationServices(configuration);
        services.AddPersistenceServices(configuration);
        services.AddInfrastructureServices(configuration);
        services.AddIdentityServices(configuration);
        services.AddApplicationServices(configuration);

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
