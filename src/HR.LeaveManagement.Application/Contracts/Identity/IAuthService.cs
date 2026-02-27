using HR.LeaveManagement.Application.Model.Identity;

namespace HR.LeaveManagement.Application.Contracts.Identity;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
}
