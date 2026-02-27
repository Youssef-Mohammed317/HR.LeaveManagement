using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.DatabaseContext;

public class HrDatabaseContext(DbContextOptions<HrDatabaseContext> options, ICurrentUserService userService) : DbContext(options)
{
    public DbSet<LeaveType> LeaveTypes { get; set; }
    public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HrDatabaseContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = base.ChangeTracker.Entries<IAuditable>()
            .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.DateModified = DateTime.UtcNow;
            entry.Entity.ModifiedBy = userService.UserId;
            if (entry.State == EntityState.Added)
            {
                entry.Entity.DateCreated = DateTime.UtcNow;
                entry.Entity.CreatedBy = userService.UserId;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
