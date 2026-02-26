using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Model.Identity;
using HR.LeaveManagement.Domain.Identity;
using HR.LeaveManagement.Domain.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HR.LeaveManagement.Identity.Services;

public class UserService(UserManager<ApplicationUser> userManager,
    IHttpContextAccessor httpContextAccessor) : IUserService
{
    public string? UserId => httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public string? Email => httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
    public bool IsAdmin => UserId != null && IsInRole(Roles.Administrator);
    public bool IsInRole(string role)
    {
        return httpContextAccessor?.HttpContext?
            .User?
            .IsInRole(role) ?? false;
    }


    public async Task<Employee> GetEmployeeByIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId)
             ?? throw new NotFoundException(nameof(ApplicationUser), userId);
        return new Employee
        {
            Id = user.Id,
            Email = user.Email!,
            Firstname = user.FirstName,
            Lastname = user.LastName,
        };
    }

    public async Task<IReadOnlyList<Employee>> GetAllEmployeesAsync()
    {
        var users = await userManager.GetUsersInRoleAsync(Roles.Employee);

        return users.Select(user => new Employee
        {
            Id = user.Id,
            Email = user.Email!,
            Firstname = user.FirstName,
            Lastname = user.LastName,
        }).ToList();

    }

}
public class CurrentUserService(IHttpContextAccessor accessor)
    : ICurrentUserService
{
    public string? UserId =>
        accessor.HttpContext?.User?
        .FindFirst(ClaimTypes.NameIdentifier)?.Value;
}