namespace HR.LeaveManagement.Domain.Common;

public abstract class BaseEntity<TKey> : Auditable
{
    public TKey Id { get; set; } = default!;

}
