using HR.LeaveManagement.Application.Contracts.Common;
using HR.LeaveManagement.Identity.IdentityContext;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.API.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<HrDatabaseContext>();
        await context.Database.MigrateAsync();

        var identityContext = services.GetRequiredService<HrIdentityDatabaseContext>();
        await identityContext.Database.MigrateAsync();

        var initializers = services.GetServices<IDbInitializer>();

        foreach (var initializer in initializers)
        {
            await initializer.InitializeAsync();
        }
    }
}
