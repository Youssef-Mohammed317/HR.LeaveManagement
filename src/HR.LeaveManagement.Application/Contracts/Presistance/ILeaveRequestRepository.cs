using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.Contracts.Presistance;

public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest, int>
{

}
