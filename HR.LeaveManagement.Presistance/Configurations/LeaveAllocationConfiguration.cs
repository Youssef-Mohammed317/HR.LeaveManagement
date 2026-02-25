using HR.LeaveManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.LeaveManagement.Persistence.Configurations;

public class LeaveAllocationConfiguration : IEntityTypeConfiguration<LeaveAllocation>
{
    public void Configure(EntityTypeBuilder<LeaveAllocation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.NumberOfDays)
               .IsRequired();

        builder.Property(x => x.Period)
               .IsRequired();
        builder.HasIndex(x => x.EmployeeId);
        builder.HasIndex(x => new { x.EmployeeId, x.LeaveTypeId, x.Period })
       .IsUnique();
    }
}
