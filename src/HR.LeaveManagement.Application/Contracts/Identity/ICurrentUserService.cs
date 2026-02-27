namespace HR.LeaveManagement.Application.Contracts.Identity;

public interface ICurrentUserService
{
    string? UserId { get; }
}