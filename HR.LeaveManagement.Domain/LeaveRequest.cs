using HR.LeaveManagement.Domain.Common;
using HR.LeaveManagement.Domain.Identity;

namespace HR.LeaveManagement.Domain;

public class LeaveRequest : BaseEntity<int>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public DateTime DateRequested { get; set; }
    public string? RequestComments { get; set; }
    public LeaveRequestStatus Status { get; set; }
    public bool Cancelled { get; set; }

    public string RequestingEmployeeId { get; set; } = string.Empty;

    public int LeaveTypeId { get; set; }
    public LeaveType LeaveType { get; set; } = null!;
}
public enum LeaveRequestStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    Cancelled = 3
}
