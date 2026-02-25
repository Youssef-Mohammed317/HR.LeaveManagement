namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;

public class LeaveTypeDto
{
    public int Id { get; set; } = default!;
    public string Name { get; set; } = string.Empty;
    public int DefaultDays { get; set; }

}
