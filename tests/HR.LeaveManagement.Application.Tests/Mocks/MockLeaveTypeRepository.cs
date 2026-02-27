using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Domain;
using Moq;

namespace HR.LeaveManagement.Application.Tests.Mocks;

public class MockLeaveTypeRepository
{
    public static Mock<ILeaveTypeRepository> GetMockeaaveTypeRepository()
    {
        var leaveTypes = new List<LeaveType>
                    {
                        new LeaveType
                        {
                            Id = 1,
                            Name = "Annual Leave",
                            DefaultDays = 21
                        },
                        new LeaveType
                        {
                            Id = 2,
                            Name = "Sick Leave",
                            DefaultDays = 10
                        },
                        new LeaveType
                        {
                            Id = 3,
                            Name = "Maternity Leave",
                            DefaultDays = 90
                        }
                    };


        var mockRepo = new Mock<ILeaveTypeRepository>();

        mockRepo.Setup(r => r.GetAsync()).ReturnsAsync(leaveTypes);

        mockRepo.Setup(r => r.CreateAsync(It.IsAny<LeaveType>())).Returns((LeaveType createdLeaveType) =>
        {
            leaveTypes.Add(createdLeaveType);
            return Task.CompletedTask;
        });
        mockRepo.Setup(r => r.UpdateAsync(It.IsAny<LeaveType>())).Returns((LeaveType updatedLeaveType) =>
        {
            var existingLeaveType = leaveTypes.FirstOrDefault(r => r.Id == updatedLeaveType.Id);
            if (existingLeaveType != null)
            {
                existingLeaveType.Name = updatedLeaveType.Name;
                existingLeaveType.DefaultDays = updatedLeaveType.DefaultDays;
            }
            return Task.CompletedTask;
        });

        mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => leaveTypes.FirstOrDefault(r => r.Id == id));

        mockRepo.Setup(r => r.DeleteAsync(It.IsAny<LeaveType>())).Returns((LeaveType l) =>
        {
            leaveTypes.Remove(l);
            return Task.CompletedTask;
        });
        return mockRepo;
    }
}
