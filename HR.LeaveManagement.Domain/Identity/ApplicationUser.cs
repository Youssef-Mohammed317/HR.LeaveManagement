
using HR.LeaveManagement.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace HR.LeaveManagement.Domain.Identity;

public class ApplicationUser : IdentityUser, IAuditable
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
}
