using HR.LeaveManagement.Domain.Common;

namespace HR.LeaveManagement.Domain;

public class LeaveType : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public int DefaultDays { get; set; }

}
