namespace HR.LeaveManagement.Domain.Common;

public abstract class Auditable : IAuditable
{
    public DateTime? DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
}