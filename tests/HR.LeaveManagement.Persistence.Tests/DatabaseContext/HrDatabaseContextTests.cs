using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using Xunit;

namespace HR.LeaveManagement.Persistence.DatabaseContext.Tests
{
    public class HrDatabaseContextTests
    {
        private HrDatabaseContext context;

        public HrDatabaseContextTests()
        {
            var dbOptions = new DbContextOptionsBuilder<HrDatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var currentUserMock = new Mock<ICurrentUserService>();
            currentUserMock.Setup(x => x.UserId).Returns("TestUser");
            context = new HrDatabaseContext(dbOptions, currentUserMock.Object);
        }
        [Fact()]
        public async Task SaveChangesAsync_Should_Set_Audit_Fields()
        {
            var leaveType = new LeaveType
            {
                DefaultDays = 7,
                Name = "Test Vacation"
            };

            await context.LeaveTypes.AddAsync(leaveType);
            await context.SaveChangesAsync();

            leaveType.DateCreated.ShouldNotBeNull();
            leaveType.DateModified.ShouldNotBeNull();
            leaveType.DateModified.Value.ToShortDateString().ShouldBe(leaveType.DateCreated.Value.ToShortDateString());
            leaveType.CreatedBy.ShouldBe("TestUser");
            leaveType.ModifiedBy.ShouldBe("TestUser");
        }
        [Fact()]
        public async Task SaveChangesAsync_Should_adjust_Audit_Fields()
        {
            var leaveType = new LeaveType
            {
                DefaultDays = 16,
                Name = "Test Vacation"
            };

            // Add
            await context.LeaveTypes.AddAsync(leaveType);
            await context.SaveChangesAsync();

            var createdDate = leaveType.DateCreated.Value;

            // Modify
            leaveType.DefaultDays = 20;
            await context.SaveChangesAsync();

            leaveType.DateModified.ShouldNotBeNull();
            leaveType.DateModified.Value.ShouldBeGreaterThan(createdDate);
            leaveType.ModifiedBy.ShouldBe("TestUser");
        }
    }
}