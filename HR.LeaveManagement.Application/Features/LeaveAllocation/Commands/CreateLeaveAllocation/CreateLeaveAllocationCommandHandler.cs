using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation;

public class CreateLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository,
    ILeaveTypeRepository leaveTypeRepository,
    IUserService userService) : IRequestHandler<CreateLeaveAllocationCommand, int>
{
    public async Task<int> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
    {

        var leaveType = await leaveTypeRepository.GetByIdAsync(request.LeaveTypeId)
                ?? throw new NotFoundException(nameof(Domain.LeaveType), request.LeaveTypeId);

        var employees = await userService.GetAllEmployeesAsync();

        var currentYear = DateTime.UtcNow.Year;

        foreach (var employee in employees)
        {
            var exists = await leaveAllocationRepository.ExsistsAsync(filter: r =>
                  r.LeaveTypeId == leaveType.Id &&
                  r.EmployeeId == employee.Id &&
                  r.Period == currentYear);

            if (!exists)
            {
                var newAllocation = new Domain.LeaveAllocation
                {
                    EmployeeId = employee.Id,
                    LeaveTypeId = leaveType.Id,
                    NumberOfDays = leaveType.DefaultDays,
                    Period = currentYear
                };

                await leaveAllocationRepository.CreateAsync(newAllocation);
            }
        }
        return leaveType.Id;
    }
}