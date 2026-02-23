using HR.LeaveManagement.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR.LeaveManagement.Domain;

public class LeaveAllocation : BaseEntity<int>
{
    public int NumberOfDays { get; set; }
    public int Period { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    // [ForeignKey(nameof(EmployeeId))]
    // Employee prop here
    public int LeaveTypeId { get; set; }
    [ForeignKey(nameof(LeaveTypeId))]
    public LeaveType LeaveType { get; set; } = null!;
}
