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

        builder.HasKey(x => x.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.DefaultDays)

            .IsRequired();

        builder.HasIndex(p => p.Name).IsUnique();

        builder.HasMany(p => p.LeaveAllocations)
            .WithOne(p => p.LeaveType)
            .HasForeignKey(p => p.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.LeaveRequests)
            .WithOne(p => p.LeaveType)
            .HasForeignKey(p => p.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
