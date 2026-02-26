using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;

public class LeaveRequestDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime DateRequested { get; set; }
    public string? RequestComments { get; set; }
    public string Status { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public int LeaveTypeId { get; set; }
    public LeaveTypeDto LeaveType { get; set; } = null!;
}