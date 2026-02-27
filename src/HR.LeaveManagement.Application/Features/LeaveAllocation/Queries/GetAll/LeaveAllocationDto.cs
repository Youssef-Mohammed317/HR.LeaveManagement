using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.Model.Identity;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAll;

public class LeaveAllocationDto
{
    public int Id { get; set; } = default!;
    public int NumberOfDays { get; set; }
    public int Period { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public int LeaveTypeId { get; set; }
    public LeaveTypeDto LeaveType { get; set; } = null!;
}