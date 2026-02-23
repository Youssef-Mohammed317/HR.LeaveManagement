namespace HR.LeaveManagement.Domain.Common;

public abstract class BaseEntity<TKey> : IAuditable
{
    public TKey Id { get; set; } = default!;
    public DateTime? DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public string? CreatedBy { get; set; } 
    public string? ModifiedBy { get; set; }
}
