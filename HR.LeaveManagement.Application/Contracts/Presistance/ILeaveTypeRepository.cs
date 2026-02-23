using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.Contracts.Presistance;

public interface ILeaveTypeRepository : IGenericRepository<LeaveType, int>
{
}
