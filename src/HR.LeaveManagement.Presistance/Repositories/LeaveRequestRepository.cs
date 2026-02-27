using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;

namespace HR.LeaveManagement.Persistence.Repositories;

public class LeaveRequestRepository(HrDatabaseContext context) : GenericRepository<LeaveRequest, int>(context), ILeaveRequestRepository
{
}