using HR.LeaveManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.LeaveManagement.Persistence.Configurations;

public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
{
    public void Configure(EntityTypeBuilder<LeaveType> builder)
    {
        builder.HasData(
            new LeaveType
            {
                Id = 1,
                Name = "Vacation",
                DefaultDays = 10,
                DateCreated = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                DateModified = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),

            },
            new LeaveType
            {
                Id = 2,
                Name = "Annual",
                DefaultDays = 21,
                DateCreated = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                DateModified = new DateTime(2024, 5, 1, 0, 0, 0, DateTimeKind.Utc)
            });

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}
