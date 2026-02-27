namespace HR.LeaveManagement.Application.Model.Identity;

public class RegistrationResponse(string userId)
{
    public string UserId { get; set; } = userId;
}
