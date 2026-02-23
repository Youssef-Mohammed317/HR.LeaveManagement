using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.Contracts.Presistance;

public interface ILeaveAllocationRepository : IGenericRepository<LeaveAllocation, int>
{

}