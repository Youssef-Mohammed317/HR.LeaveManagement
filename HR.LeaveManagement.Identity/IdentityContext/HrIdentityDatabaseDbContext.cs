using HR.LeaveManagement.Domain.Common;
using HR.LeaveManagement.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Identity.IdentityContext;

public class HrIdentityDatabaseContext(DbContextOptions<HrIdentityDatabaseContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HrIdentityDatabaseContext).Assembly);

    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = base.ChangeTracker.Entries<IAuditable>()
            .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.DateModified = DateTime.UtcNow;
            if (entry.State == EntityState.Added)
            {
                entry.Entity.DateCreated = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
