using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.Model.Identity;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocationDetails;

public class LeaveAllocationDetailsDto
{
    public int Id { get; set; } = default!;
    public DateTime? DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public int NumberOfDays { get; set; }
    public int Period { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public Employee Employee { get; set; } = null!;
    public int LeaveTypeId { get; set; }
    public LeaveTypeDto LeaveType { get; set; } = null!;
}