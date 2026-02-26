using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Presistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Model.Email;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation;

public class DeleteLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository,
    IUserService userService, IEmailSender emailSender,
    ILeaveRequestRepository leaveRequestRepository)
    : IRequestHandler<DeleteLeaveAllocationCommand>
{
    public async Task Handle(DeleteLeaveAllocationCommand request, CancellationToken cancellationToken)
    {
        if (!userService.IsAdmin)
            throw new ForbiddenAccessException();

        var entity = await leaveAllocationRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(LeaveAllocation), request.Id);

        var hasLeaveRequest = await leaveRequestRepository.ExistsAsync(r =>
            r.EmployeeId == entity.EmployeeId &&
            r.LeaveTypeId == entity.LeaveTypeId &&
            r.Status == LeaveRequestStatus.Approved &&
            r.StartDate.Year == entity.Period);

        if (hasLeaveRequest)
            throw new BadRequestException(
                "Cannot delete this allocation because there are approved leave requests for this period.");

        await leaveAllocationRepository.DeleteAsync(entity);

    }
}