using HR.LeaveManagement.Domain.Common;
using HR.LeaveManagement.Domain.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR.LeaveManagement.Domain;

public class LeaveAllocation : BaseEntity<int>
{
    public int NumberOfDays { get; set; }
    public int Period { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public int LeaveTypeId { get; set; }
    public LeaveType LeaveType { get; set; } = null!;
}
