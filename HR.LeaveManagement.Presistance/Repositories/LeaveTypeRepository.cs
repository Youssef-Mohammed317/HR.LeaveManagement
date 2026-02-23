using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;

namespace HR.LeaveManagement.Persistence.Repositories;

public class LeaveTypeRepository(HrDatabaseContext context) : GenericRepository<LeaveType, int>(context), ILeaveTypeRepository
{
}
