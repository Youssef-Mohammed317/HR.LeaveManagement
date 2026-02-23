using HR.LeaveManagement.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR.LeaveManagement.Domain;

public class LeaveRequest : BaseEntity<int>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public DateTime DateRequested { get; set; }
    public string? RequestComments { get; set; }
    public bool? Approved { get; set; }
    public bool Cancelled { get; set; }

    public string RequestingEmployeeId { get; set; } = string.Empty;
    // [ForeignKey(nameof(RequestingEmployeeId))]
    // Employee nav prop here

    public int LeaveTypeId { get; set; }
    [ForeignKey(nameof(LeaveTypeId))]
    public LeaveType LeaveType { get; set; } = null!;
}
