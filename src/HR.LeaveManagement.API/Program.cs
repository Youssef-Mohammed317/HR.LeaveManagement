using HR.LeaveManagement.API.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting HR.LeaveManagement API...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    });

    builder.Services.AddApiServices(builder.Configuration);

    var app = builder.Build();

    Log.Information("Application started successfully in {Environment}",
    app.Environment.EnvironmentName);
    await app.InitialiseDatabaseAsync();

    app.UsePresentationPipeline();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}