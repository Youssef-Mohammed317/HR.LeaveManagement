using HR.LeaveManagement.Application.Model.Identity;

namespace HR.LeaveManagement.Application.Contracts.Identity;

public interface IUserService
{
    string? UserId { get; }
    bool IsInRole(string role);

    Task<IReadOnlyList<Employee>> GetAllEmployeesAsync();
    Task<Employee> GetEmployeeByIdAsync(string userId);
}
public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
}
